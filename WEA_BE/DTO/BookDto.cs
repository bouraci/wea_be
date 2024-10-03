using EFModels.Models;

namespace WEA_BE.DTO;

public class BookDto
{
    public BookDto(Book book)
    {
        this.Id = book.Id;
        this.Title = book.Title;
        this.Authors = book.Authors;
        this.Publisher = book.Publisher;
        this.PublishedDate = book.PublishedDate;
        this.ISBN = book.ISBN;
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Authors { get; set; }
    public string Publisher { get; set; }
    public DateTime PublishedDate { get; set; }
    public int ISBN { get; set; }
}
