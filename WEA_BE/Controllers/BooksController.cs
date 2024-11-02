using Microsoft.AspNetCore.Mvc;
using WEA_BE.DTO;
using WEA_BE.Services;

namespace WEA_BE.Controllers;

/// <summary>
/// API Controller pro správu knih.
/// </summary>
[Route("books")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private readonly IBookService _bookService;

    /// <summary>
    /// Konstruktor pro BooksController.
    /// </summary>
    /// <param name="logger">Služba pro logování chyb a informací.</param>
    /// <param name="bookService">Služba pro správu knih.</param>
    public BooksController(ILogger<BooksController> logger, IBookService bookService)
    {
        _logger = logger;
        _bookService = bookService;
    }

    /// <summary>
    /// Endpoint pro získání knih na základě různých filtrů.
    /// </summary>
    /// <param name="title">Název knihy pro filtrování.</param>
    /// <param name="author">Autor knihy pro filtrování.</param>
    /// <param name="genre">Žánr knihy pro filtrování.</param>
    /// <param name="publicationYear">Rok vydání knihy pro filtrování.</param>
    /// <param name="minRating">Minimální hodnocení pro filtrování.</param>
    /// <param name="maxRating">Maximální hodnocení pro filtrování.</param>
    /// <param name="page">Číslo stránky pro stránkování (výchozí hodnota je 1).</param>
    /// <param name="pageSize">Velikost stránky pro stránkování (výchozí hodnota je 10).</param>
    /// <returns>Vrací seznam knih s informacemi o stránkování.</returns>
    [HttpGet]
    [ProducesResponseType<BooksResponse>(StatusCodes.Status200OK)]
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
        (List<BookSimpleDto> books, int totalRecords) = _bookService.GetBooks(title, author, genre, publicationYear, minRating, maxRating, page, pageSize);
        var response = new BooksResponse()
        {
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
            Page = page,
            PageSize = pageSize,
            Books = books
        };

        return Ok(response);
    }

    /// <summary>
    /// Endpoint pro získání knihy podle jejího ID.
    /// </summary>
    /// <param name="id">ID knihy.</param>
    /// <returns>Vrací podrobnosti o knize nebo NotFound, pokud kniha neexistuje.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType<BookDto>(StatusCodes.Status200OK)]
    public IActionResult Get([FromRoute] int id)
    {
        BookDto book = _bookService.GetBookById(id);
        if (book == null)
            return NotFound();

        return Ok(book);
    }

    /// <summary>
    /// Endpoint pro získání unikátních žánrů
    /// </summary>
    /// <returns>List unikátních žánrů</returns>
    [HttpGet("genres")]
    public IActionResult GetGenres()
    {
        var genres = _bookService.GetUniqueGenres();
        return Ok(genres);
    }

}
