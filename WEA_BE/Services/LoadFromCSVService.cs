using CsvHelper;
using CsvHelper.Configuration;
using EFModels.Data;
using EFModels.Models;
using System.Globalization;

namespace WEA_BE.Services;

/// <summary>
/// Statická třída poskytující službu pro načítání dat knih z CSV souboru.
/// </summary>
public static class LoadFromCsvService
{
    /// <summary>
    /// Načte data knih z CSV souboru a uloží je do databáze.
    /// </summary>
    /// <param name="filePath">Cesta k CSV souboru, který obsahuje data knih.</param>
    /// <param name="ctx">Kontext databáze, který se používá k uložení dat.</param>
    /// <returns>Asynchronní úloha, která indikuje, kdy bylo načtení a uložení dat dokončeno.</returns>

    public static async Task LoadFromCSV(string filePath, DatabaseContext ctx)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            MissingFieldFound = null
        };
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Context.RegisterClassMap<BookMap>();
            IEnumerable<Book> records = csv.GetRecords<Book>();

            records = records.Where(x => ctx.Books.Select(y => y.ISBN13).Contains(x.ISBN13) == false);
            ctx.Books.AddRange(records);
            await ctx.SaveChangesAsync();
        }
    }
}
public sealed class BookMap : ClassMap<Book>
{
    public BookMap()
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            MissingFieldFound = null
        };
        AutoMap(config);
        Map(m => m.Id).Ignore();
        Map(m => m.IsHidden).Ignore();
        Map(m => m.ISBN10).Index(0);           // First column in CSV
        Map(m => m.ISBN13).Index(1);           // Second column
        Map(m => m.Title).Index(2);            // Third column
        Map(m => m.Subtitle).Index(3);         // Fourth column (can be empty)
        Map(m => m.Authors).Index(4);          // Fifth column
        Map(m => m.Genre).Index(5);            // Sixth column
        Map(m => m.CoverImageUrl).Index(6);    // Seventh column
        Map(m => m.Description).Index(7);      // Eighth column
        Map(m => m.PublicationYear).Index(8).Default(0);   // Ninth column
        Map(m => m.Rating).Index(9).Default(0.0);           // Tenth column
        Map(m => m.PageCount).Index(10).Default(0);        // Eleventh column
        Map(m => m.TotalRatings).Index(11).Default(0);    // Twelfth column
    }
}