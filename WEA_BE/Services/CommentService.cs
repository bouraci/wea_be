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

    public bool AddComment(int bookId, string content, string userName)
    {
        var user = _ctx.Users.SingleOrDefault(user => user.UserName == userName);
        if (user == null)
        {
            return false;
        }
        var comment = new Comment()
        {
            Content = content,
            CreatedDate = DateTime.Now,
            BookId = bookId,
            UserId = user.Id
        };
        _ctx.Comments.Add(comment);
        _ctx.SaveChanges();
        return true;
    }
}
