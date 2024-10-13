namespace CommentarySystem.Server.Data.Entities;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}