using File = CommentarySystem.Server.Data.Entities.File;

namespace CommentarySystem.Server.Data.Entities;

public class Comment
{
    public int CommentId { get; set; }
    public string Text { get; set; }
    public int UserId { get; set; }
    public int? ParentCommentId { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; }

    public ICollection<File>? Files { get; set; }
    public Comment? ParentComment { get; set; }
    public ICollection<Comment>? ChildComments { get; set; }
    public IEnumerable<SelectedRange> SelectedRandges { get; set; } = new List<SelectedRange>();
}
