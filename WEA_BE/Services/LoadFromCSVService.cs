using CsvHelper;
using CsvHelper.Configuration;
using EFModels.Data;
using EFModels.Models;
using System.Globalization;
using WEA_BE.Models;

namespace WEA_BE.Services;

/// <summary>
/// Statická třída poskytující službu pro načítání dat knih z CSV souboru.
/// </summary>
public class LoadFromCsvService
{
    private readonly DatabaseContext _ctx;
    public LoadFromCsvService(DatabaseContext ctx)
    {
        _ctx = ctx;
    }
    /// <summary>
    /// Načte data knih z CSV souboru a uloží je do databáze.
    /// </summary>
    /// <param name="filePath">Cesta k CSV souboru, který obsahuje data knih.</param>
    /// <param name="ctx">Kontext databáze, který se používá k uložení dat.</param>
    /// <returns>Asynchronní úloha, která indikuje, kdy bylo načtení a uložení dat dokončeno.</returns>

    public async Task LoadFromCSV(string filePath)
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

            records = records.Where(x => _ctx.Books.Select(y => y.ISBN13).Contains(x.ISBN13) == false);
            _ctx.Books.AddRange(records);
            await _ctx.SaveChangesAsync();
        }
    }
}
