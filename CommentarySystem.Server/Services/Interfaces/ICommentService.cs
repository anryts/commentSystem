using CommentarySystem.Server.Data.Entities;
using CommentarySystem.Server.Model;

namespace CommentarySystem.Server.Services.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentResponseModel>> GetCommentsAsync(int page, int pageSize, string filterBy,
        string sortBy);

    //Task AddReplyAsync(CommentReplyModel model);
    Task<Comment> GetCommentByIdAsync(int id);
    Task AddCommentAsync(CommentCreationModel model, ICollection<IFormFile>? files);
    Task DeleteCommentAsync(int id);
    Task UpdateCommentAsync(CommentUpdateModel model);
}