using EFModels.Models;
using Microsoft.EntityFrameworkCore;

namespace EFModels.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options) { }

    public DbSet<Book> Books { get; set; }
}
