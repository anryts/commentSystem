namespace CommentarySystem.Server.Data.Entities;

public class SelectedRange
{
    public int Id { get; set; }
    public int StartIndex { get; set; }
    public int EndIndex { get; set; }

    public int CommentId { get; set; }
    public Comment Comment { get; set; }
}