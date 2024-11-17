using WEA_BE.DTO;

namespace WEA_BE.Services;

public interface ICommentService
{
    bool AddComment(int bookId, string content, string userName, double rating);
    List<CommentDto> GetComments(int bookId);

    public bool HasUserRating(int bookId, string userName);
}