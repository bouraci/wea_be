using AutoMapper;
using EFModels.Data;
using EFModels.Models;
using Microsoft.EntityFrameworkCore;
using WEA_BE.DTO;

namespace WEA_BE.Services;

public class CommentService : ICommentService
{
    private readonly DatabaseContext _ctx;
    private readonly IMapper _mapper;
    public CommentService(DatabaseContext ctx, IMapper mapper)
    {
        _ctx = ctx;
        _mapper = mapper;
    }
    public List<CommentDto> GetComments(int bookId)
    {
        IEnumerable<Comment> comments = _ctx.Comments
            .Where(comment => comment.BookId == bookId)
            .Include(x => x.User);
        var commentsDtos = _mapper.Map<List<CommentDto>>(comments);
        return commentsDtos;
    }

    public bool AddComment(int bookId, string content, string userName, double rating)
    {
        if (rating < 0 || rating > 5)
        {
            throw new ArgumentException("Rating must be between 0 and 5.");
        }

        var user = _ctx.Users.SingleOrDefault(user => user.UserName == userName);
        if (user == null)
        {
            return false;
        }

        bool hasRated = HasUserRating(bookId, userName);
        if (hasRated)
        {
            throw new InvalidOperationException("User has already rated this book.");
        }


        var comment = new Comment()
        {
            Content = content,
            CreatedDate = DateTime.Now,
            BookId = bookId,
            UserId = user.Id,
            Rating = rating
        };
        _ctx.Comments.Add(comment);
        var book = _ctx.Books.SingleOrDefault(b => b.Id == bookId);
        if (book != null)
        {
            book.TotalRatings++;
            book.Rating = Math.Round((book.Rating * (book.TotalRatings - 1) + rating) / book.TotalRatings, 1);
        }
        _ctx.SaveChanges();
        return true;
    }

    public bool HasUserRating(int bookId, string userName)
    {
        var user = _ctx.Users.SingleOrDefault(u => u.UserName == userName);
        if (user == null) return false;

        return _ctx.Comments.Any(c => c.BookId == bookId && c.UserId == user.Id && c.Rating > 0);
    }
}
