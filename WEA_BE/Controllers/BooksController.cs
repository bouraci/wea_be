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
    public BooksController(ILogger<BooksController> logger, DatabaseContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    [HttpGet]
    public List<BookDto> Get()
    {
        List<BookDto> books = new List<BookDto>();
        foreach (var book in _ctx.books)
        {
            var bookDto = new BookDto(book);
            books.Add(bookDto);
        }

        return books;
    }

    [HttpGet("{id}")]
    public BookDto Get([FromRoute] int id)
    {
        var book = _ctx.books.SingleOrDefault(x => x.Id == id);
        var bookDto = new BookDto(book);
        return bookDto;
    }

}
