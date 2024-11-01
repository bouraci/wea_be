using Microsoft.AspNetCore.Mvc;
using WEA_BE.DTO;
using WEA_BE.Services;

namespace WEA_BE.Controllers;

[Route("comments")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly ILogger<CommentController> _logger;

    public CommentController(ICommentService commentService, ILogger<CommentController> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    [HttpPost("comment")]
    public IActionResult Add([FromBody] CommentRequestDto commentRequest)
    {
        _logger.LogInformation("Recieved request:");
        _logger.LogInformation(Request.ToString());
        bool result = _commentService.AddComment(commentRequest.bookId, commentRequest.content, commentRequest.userName);
        if (result)
        {
            _logger.LogInformation("Added comment");
            return Ok("Comment added");
        }
        _logger.LogInformation("Could not add comment");
        return BadRequest("Comment could not be added");
    }
}
