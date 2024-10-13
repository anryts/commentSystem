using File = CommentarySystem.Server.Data.Entities.File;

namespace CommentarySystem.Server.Model;

public record CommentResponseModel
{
    public int CommentId { get; init; }
    public string Text { get; init; }
    public string UserName { get; init; }
    public string UserEmail { get; set; }
    public DateTime CreatedAt { get; init; }
    public int? ParentCommentId { get; set; }
    public IEnumerable<SelectedRangeResponseModel>? SelectedRange { get; set; }
    public IEnumerable<CommentResponseModel>? Replies { get; set; }
    public IEnumerable<FileResponse>? Files { get; set; }
}

public record SelectedRangeResponseModel(int StartIndex, int EndIndex);

public record FileResponse
{
    public int CommentId { get; set; }
    public string FileName { get; set; }
    public string Content { get; set; }
    public string FileType { get; set; }
};