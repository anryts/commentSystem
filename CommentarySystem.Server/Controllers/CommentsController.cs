using CommentarySystem.Server.Model;
using CommentarySystem.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommentarySystem.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    //TODO: add pagination
    [HttpGet()]
    public async Task<IActionResult> GetComments(int page = 1, int pageSize = 25, string filterBy = "date",
        string sortBy = "desc")
    {
        var comments = await _commentService
            .GetCommentsAsync(page, pageSize, filterBy, sortBy);
        return Ok(comments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment(int id)
    {
        var comment = await _commentService.GetCommentByIdAsync(id);

        return Ok(comment);
    }

    [HttpPost]
    public async Task<IActionResult> PostComment([FromForm]ICollection<IFormFile>? files, [FromForm]CommentCreationModel comment)
    {
        //TODO: add validation by fluent validation
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _commentService.AddCommentAsync(comment, files);
        return new OkObjectResult("Comment added successfully");
    }

    // [HttpPost("reply")]
    // public async Task<IActionResult> PostReply([FromBody] CommentReplyModel comment)
    // {
    //     //TODO: add validation by fluent validation
    //     await _commentService.AddReplyAsync(comment);
    //     return new OkObjectResult("Reply added successfully");
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment([FromBody] CommentUpdateModel model)
    {
        //TODO: add validation by fluent validation
        await _commentService.UpdateCommentAsync(model);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _commentService.GetCommentByIdAsync(id);

        await _commentService.DeleteCommentAsync(id);
        return NoContent();
    }
}