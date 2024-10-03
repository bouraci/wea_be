using EFModels.Data;
using EFModels.Models;
using Microsoft.AspNetCore.Mvc;
using WEA_BE.DTO;

namespace WEA_BE.Controllers;
[ApiController]
[Route("dbtest")]
public class DbTestController : ControllerBase
{
    private readonly ILogger<DbTestController> _logger;
    private readonly DatabaseContext _ctx;

    public DbTestController(ILogger<DbTestController> logger, DatabaseContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    [HttpPost(Name = "TestDb")]
    public BookDto Get(string name, string authors)
    {
        Random rnd = new Random();

        var book = new Book()
        {
            Title = name,
            Authors = authors,
            Publisher = "Bouraci",
            PublishedDate = DateTime.Now,
            ISBN = rnd.Next(100)

        };
        //write to Db
        _ctx.books.Add(book);
        _ctx.SaveChanges();
        //read from Db
        var bookFromDb = _ctx.books.SingleOrDefault(x => x.Id == book.Id);
        var bookDto = new BookDto(bookFromDb);
        return bookDto;
    }
}
