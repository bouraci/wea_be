using System.ComponentModel.DataAnnotations;

namespace EFModels.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<Comment> Comments { get; set; }
    public ICollection<Book> FavouriteBooks { get; set; } = new HashSet<Book>();
    public override string ToString()
    {
        return UserName;
    }
}
