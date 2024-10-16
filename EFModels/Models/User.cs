using System.ComponentModel.DataAnnotations;

namespace EFModels.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public override string ToString()
    {
        return UserName;
    }
}
