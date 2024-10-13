using CommentarySystem.Server.Services.Interfaces;
using WebApplication1.Data;
using File = CommentarySystem.Server.Data.Entities.File;

namespace CommentarySystem.Server.Services;

public class FileService : IFileService
{
    private readonly AppDbContext _context;

    public FileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveFileAsync(IFormFile file, int commentId)
    {
        ArgumentNullException.ThrowIfNull(file);

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var fileEntity = new File
            {
                Content = Convert.ToBase64String(memoryStream.ToArray()),
                CommentId = commentId,
                FileName = file.FileName,
                FileType = file.ContentType
            };
            await _context.File.AddAsync(fileEntity);
            await _context.SaveChangesAsync();
        }
    }

    public Task<string> SaveFileAsync(byte[] file, string folderName, string extension)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GetFileAsync(string fileName, string folderName)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFileAsync(string fileName, string folderName)
    {
        throw new NotImplementedException();
    }
}