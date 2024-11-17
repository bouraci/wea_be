namespace WEA_BE.DTO;

public class UserDetailDto
{
    public string? Address { get; set; }
    public string? BillingAddress { get; set; }
    public bool? ProcessData { get; set; }
    public bool? IsMale { get; set; } //Genesis 1:27
    public int? Age { get; set; }
    public List<string> FavouriteGerners { get; set; }
    public string? Refferal { get; set; }
}
