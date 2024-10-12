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
        [FromQuery] string title,
        [FromQuery] string author,
        [FromQuery] string genre,
        [FromQuery] int? publicationYear,
        [FromQuery] double? minRating,
        [FromQuery] double? maxRating,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageSize > 100) pageSize = 100;

        var query = _ctx.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(b => b.Title.ToLower()
                                            .Contains(title.Trim().ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            query = query.Where(b => b.Authors.ToLower()
                                              .Contains(author.Trim().ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(b => b.Genre.ToLower()
                                            .Contains(genre.Trim().ToLower()));
        }

        if (publicationYear is not null)
        {
            query = query.Where(b => b.PublicationYear == publicationYear);
        }

        if (minRating is not null)
        {
            query = query.Where(b => b.Rating >= minRating);
        }

        if (maxRating is not null)
        {
            query = query.Where(b => b.Rating <= maxRating);
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
