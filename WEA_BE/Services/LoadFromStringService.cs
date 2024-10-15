using EFModels.Data;
using EFModels.Models;
using System.Text.Json;

namespace WEA_BE.Services;

public static class LoadFromStringService
{
    public static async Task LoadFromString(DatabaseContext ctx, string json)
    {
        var books = JsonSerializer.Deserialize<List<Book>>(json);

        ctx.Books.AddRange(books);
        await ctx.SaveChangesAsync();

    }
}
