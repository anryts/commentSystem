namespace CommentarySystem.Server.Services.Interfaces;

public interface IFileService
{
    /// <summary>
    /// Save binary data into db
    /// </summary>
    /// <param name="file"></param>
    /// <param name="commentId"></param>
    /// <returns></returns>
    Task SaveFileAsync(IFormFile file, int commentId);
    Task<string> SaveFileAsync(byte[] file, string folderName, string extension);
    Task<byte[]> GetFileAsync(string fileName, string folderName);
    Task DeleteFileAsync(string fileName, string folderName);
}