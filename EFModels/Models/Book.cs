using System.ComponentModel.DataAnnotations;

namespace EFModels.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Authors { get; set; }
    public string Publisher { get; set; }
    public DateTime PublishedDate { get; set; }
    public int ISBN { get; set; }
}
