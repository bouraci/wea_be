using EFModels.Data;
using EFModels.Models;
using System.Text.Json;

namespace WEA_BE.Services;

/// <summary>
/// Statická třída poskytující službu pro načítání dat knih z JSON řetězce.
/// </summary>
public static class LoadFromStringService
{
    /// <summary>
    /// Načte data knih z řetězce ve formátu JSON a uloží je do databáze.
    /// </summary>
    /// <param name="ctx">Kontext databáze, který se používá k uložení dat.</param>
    /// <param name="json">Řetězec obsahující data ve formátu JSON, která reprezentují seznam objektů typu Book.</param>
    /// <returns>Asynchronní úloha, která indikuje, kdy bylo načtení a uložení dat dokončeno.</returns>
    /// <exception cref="JsonException">Vyvolá se, pokud dojde k chybě při deserializaci JSON řetězce.</exception>

    public static async Task LoadFromString(DatabaseContext ctx, string json)
    {
        var books = JsonSerializer.Deserialize<List<Book>>(json);

        ctx.Books.AddRange(books);
        await ctx.SaveChangesAsync();

    }
}
