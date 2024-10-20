namespace WEA_BE.DTO;

public class RegisterRequestDto
{
    public required string Name { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }
}