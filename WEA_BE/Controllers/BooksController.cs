using AutoMapper;
using EFModels.Data;
using Microsoft.AspNetCore.Mvc;
using WEA_BE.DTO;

namespace WEA_BE.Controllers;

[Route("books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private readonly DatabaseContext _ctx;
    private readonly IMapper _mapper;
    public BooksController(ILogger<BooksController> logger, DatabaseContext ctx, IMapper mapper)
    {
        _logger = logger;
        _ctx = ctx;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get(
        [FromQuery] string? title,
        [FromQuery] string? author,
        [FromQuery] string? genre,
        [FromQuery] int? publicationYear,
        [FromQuery] double? minRating,
        [FromQuery] double? maxRating,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        
        var query = _ctx.Books.AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(b => b.Title.Contains(title) && title.Length <= 100);
        }

        if (!string.IsNullOrEmpty(author))
        {
            query = query.Where(b => b.Authors.Contains(author) && author.Length <= 100);
        }

        if (!string.IsNullOrEmpty(genre))
        {
            query = query.Where(b => b.Genre.Contains(genre) && genre.Length <= 100);
        }

        if (publicationYear.HasValue)
        {
            query = query.Where(b => b.PublicationYear == publicationYear.Value);
        }

        if (minRating.HasValue)
        {
            query = query.Where(b => b.Rating >= minRating.Value);
        }

        if (maxRating.HasValue)
        {
            query = query.Where(b => b.Rating <= maxRating.Value);
        }

        var totalRecords = query.Count();
        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        var books = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var bookDtos = _mapper.Map<List<BookDto>>(books);

        var response = new
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            Page = page,
            PageSize = pageSize,
            Books = bookDtos
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public BookDto Get([FromRoute] int id)
    {
        var book = _ctx.Books.SingleOrDefault(x => x.Id == id);
        var bookDto = _mapper.Map<BookDto>(book);
        return bookDto;
    }

}
