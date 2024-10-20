using WEA_BE.DTO;

public interface IBookService
{
    (List<BookDto>, int totalRecords) GetBooks(string? title, string? author, string? genre, int? publicationYear, double? minRating, double? maxRating, int page, int pageSize);
    BookDto GetBookById(int id);
}
