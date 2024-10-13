namespace CommentarySystem.Server.Data.Entities;

public class File
{
    public int FileId { get; set; }
    public string  FileName { get; set; }
    public string Content { get; set; }
    public int CommentId { get; set; }
    public string FileType { get; set; }
    public long FileSize { get; set; }

    public Comment Comment { get; set; }
}