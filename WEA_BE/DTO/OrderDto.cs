namespace WEA_BE.DTO;

public class OrderDto
{
    public int Id { get; set; }
    public List<BookSimpleDto> Books { get; set; }
    public DateTime Created { get; set; }
    public double totalPrice { get; set; }
}
