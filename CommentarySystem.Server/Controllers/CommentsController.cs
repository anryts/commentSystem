using CommentarySystem.Server.Model;
using CommentarySystem.Server.Services.Interfaces;
using FluentValidation;
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
    public async Task<IActionResult> PostComment([FromForm] ICollection<IFormFile>? files,
        [FromForm] CommentCreationModel comment, [FromServices] IValidator<CommentCreationModel> validator)
    {
        await validator.ValidateAndThrowAsync(comment);

        await _commentService.AddCommentAsync(comment, files);
        return new OkObjectResult("Comment added successfully");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _commentService.GetCommentByIdAsync(id);

        await _commentService.DeleteCommentAsync(id);
        return NoContent();
    }
}