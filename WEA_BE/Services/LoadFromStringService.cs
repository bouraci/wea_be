using AutoMapper;
using EFModels.Data;
using EFModels.Enums;
using EFModels.Models;
using System.Text.Json;
using WEA_BE.DTO;

namespace WEA_BE.Services;

/// <summary>
/// Statická třída poskytující službu pro načítání dat knih z JSON řetězce.
/// </summary>
public class LoadFromStringService
{
    private readonly DatabaseContext _ctx;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;
    public LoadFromStringService(DatabaseContext ctx, IMapper mapper, IAuditService auditService)
    {
        _ctx = ctx;
        _mapper = mapper;
        _auditService = auditService;
    }
    /// <summary>
    /// Načte data knih z řetězce ve formátu JSON a uloží je do databáze.
    /// </summary>
    /// <param name="ctx">Kontext databáze, který se používá k uložení dat.</param>
    /// <param name="json">Řetězec obsahující data ve formátu JSON, která reprezentují seznam objektů typu Book.</param>
    /// <returns>Asynchronní úloha, která indikuje, kdy bylo načtení a uložení dat dokončeno.</returns>
    /// <exception cref="JsonException">Vyvolá se, pokud dojde k chybě při deserializaci JSON řetězce.</exception>

    public async Task LoadFromString(List<CdbBookDto> data)
    {
        var books = _mapper.Map<List<Book>>(data);
        List<(Book book, string isbn)> group = _ctx.Books
                .Select(x => new { Book = x, ISBN = x.ISBN13 })
                .AsEnumerable()
                .Select(x => (x.Book, x.ISBN))
                .ToList();
        var isbnSet = new HashSet<string>(group.Select(x => x.isbn));
        foreach (var book in books)
        {
            if (isbnSet.Contains(book.ISBN13))
            {
                var oldBook = group.Single(x => x.isbn == book.ISBN13);
                oldBook.book.IsHidden = true;
            }
            _ctx.Add(book);
        }
        await _ctx.SaveChangesAsync();
        _auditService.LogAudit("", _mapper.Map<List<BookDto>>(books), LogType.LoadCdb, "cdb");
    }
}
