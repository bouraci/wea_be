namespace WEA_BE.DTO;

public class CdbBookDto
{
    public string ISBN10 { get; set; }
    public string ISBN13 { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string Authors { get; set; }
    public string Categories { get; set; }
    public string Thumbnail { get; set; }
    public string Description { get; set; }
    public int PublicationYear { get; set; }
    public double Rating { get; set; }
    public int PageCount { get; set; }
    public int TotalRatings { get; set; }
    public double Price { get; set; }
}
