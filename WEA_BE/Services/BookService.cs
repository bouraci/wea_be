using AutoMapper;
using EFModels.Data;
using WEA_BE.DTO;

public class BookService : IBookService
{
    private readonly DatabaseContext _ctx;
    private readonly IMapper _mapper;

    public BookService(DatabaseContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }

    public (List<BookDto>, int totalRecords) GetBooks(string? title, string? author, string? genre, int? publicationYear, double? minRating, double? maxRating, int page, int pageSize)
    {
        if (pageSize > 100) pageSize = 100;

        var query = _ctx.Books.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(b => b.Title.ToLower().Contains(title.Trim().ToLower()));

        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(b => b.Authors.ToLower().Contains(author.Trim().ToLower()));

        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(b => b.Genre.ToLower().Contains(genre.Trim().ToLower()));

        if (publicationYear is not null)
            query = query.Where(b => b.PublicationYear == publicationYear);

        if (minRating is not null)
            query = query.Where(b => b.Rating >= minRating);

        if (maxRating is not null)
            query = query.Where(b => b.Rating <= maxRating);

        var totalRecords = query.Count();

        var books = query.Skip((page - 1) * pageSize)
                         .Take(pageSize)
                         .ToList();

        var bookDtos = _mapper.Map<List<BookDto>>(books);
        return (bookDtos, totalRecords);
    }

    public BookDto GetBookById(int id)
    {
        var book = _ctx.Books.SingleOrDefault(x => x.Id == id);
        return _mapper.Map<BookDto>(book);
    }
}
