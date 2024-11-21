namespace WEA_BE.DTO;

public class RegisterRequestDto
{
    public required string Name { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }
    public AddressDto? Address { get; set; }
    public AddressDto? BillingAddress { get; set; }
    public bool? ProcessData { get; set; }
    public bool? IsMale { get; set; } //Genesis 1:27
    public DateTime? Age { get; set; }
    public List<string> FavouriteGerners { get; set; }
    public string? Referral { get; set; }
}