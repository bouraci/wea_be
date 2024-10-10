using EFModels.Data;
using EFModels.Models;
using System.Text.Json;

namespace WEA_BE.Services;

public static class LoadFromApiService
{
    public static async Task<bool> TryLoadFromApi(DatabaseContext ctx, ILogger logger)
    {
        string url = @"http://wea.nti.tul.cz:1337/data";
        using HttpClient client = new();
        var response = await client.GetAsync(url);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            logger.LogInformation("Unable to acess the Central database REST API for book data");
            return false;
        }
        var json = await response.Content.ReadAsStringAsync();
        var books = JsonSerializer.Deserialize<List<Book>>(json);
        ctx.Books.AddRange(books);
        await ctx.SaveChangesAsync();

        return true;
    }
}
