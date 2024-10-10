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
    public List<BookDto> Get()
    {
        var books = _ctx.Books.ToList();
        var bookDtos = _mapper.Map<List<BookDto>>(books);

        return bookDtos;
    }

    [HttpGet("{id}")]
    public BookDto Get([FromRoute] int id)
    {
        var book = _ctx.Books.SingleOrDefault(x => x.Id == id);
        var bookDto = _mapper.Map<BookDto>(book);
        return bookDto;
    }

}
