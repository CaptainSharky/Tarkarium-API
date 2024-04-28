using Microsoft.AspNetCore.Mvc;
using education.Comments;
using System.Threading.Tasks;

namespace education.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddCommentToUserAsync(int userId, CommentEntity comment)
        {
            bool result = await _commentService.AddCommentToUserAsync(userId, comment);
            if (result)
            {
                return Ok("Comment added to user successfully");
            }
            else
            {
                return BadRequest("Failed to add comment to user");
            }
        }

        [HttpPut("like/{commentId}")]
        public async Task<IActionResult> IncreaseLikesAsync(int commentId)
        {
            bool result = await _commentService.IncreaseLikesAsync(commentId);
            if (result)
            {
                return Ok("Likes increased successfully");
            }
            else
            {
                return BadRequest("Failed to increase likes for comment");
            }
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteCommentAsync(int commentId)
        {
            bool result = await _commentService.DeleteCommentAsync(commentId);
            if (result)
            {
                return Ok("Comment deleted successfully");
            }
            else
            {
                return BadRequest("Failed to delete comment");
            }
        }

        [HttpGet("info/{commentId}")]
        public async Task<IActionResult> GetCommentInfoAsync(int commentId)
        {
            var commentInfo = await _commentService.GetCommentInfoAsync(commentId);
            if (commentInfo != null)
            {
                return Ok(commentInfo);
            }
            else
            {
                return NotFound("Comment info not found");
            }
        }
    }
}
