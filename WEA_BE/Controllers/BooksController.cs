using Microsoft.AspNetCore.Mvc;
using WEA_BE.DTO;
using WEA_BE.Services;

namespace WEA_BE.Controllers;

[Route("books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private readonly IBookService _bookService;

    public BooksController(ILogger<BooksController> logger, IBookService bookService)
    {
        _logger = logger;
        _bookService = bookService;
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
        _logger.LogInformation("Recieved request:");
        _logger.LogInformation(Request.ToString());
        (List<BookDto> books, int totalRecords) = _bookService.GetBooks(title, author, genre, publicationYear, minRating, maxRating, page, pageSize);
        var response = new
        {
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
            Page = page,
            PageSize = pageSize,
            Books = books
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var book = _bookService.GetBookById(id);
        if (book == null)
            return NotFound();

        return Ok(book);
    }
}
