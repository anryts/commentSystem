using AutoMapper;
using CommentarySystem.Server.Data.Entities;
using CommentarySystem.Server.Model;
using CommentarySystem.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using File = CommentarySystem.Server.Data.Entities.File;

namespace CommentarySystem.Server.Services;

public class CommentService : ICommentService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public CommentService(AppDbContext context, IMapper mapper, IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<IEnumerable<CommentResponseModel>> GetCommentsAsync(int page, int pageSize, string filterBy,
        string sortOrder)
    {
        // Get all comments
        var parentComments = _context.Comment
            .Where(c => c.ParentCommentId == null) // Only include parent comments
            .Include(c => c.User)
            .Include(c => c.Files)
            .Include(c => c.ChildComments)
            .Include(c => c.Files)
            .AsQueryable();


        if (string.IsNullOrEmpty(filterBy))
        {
            throw new Exception("Filtering by this criteria is not supported");
        }

        // Apply filtering based on the filter criteria
        parentComments = filterBy.ToLower() switch
        {
            "username" => sortOrder.Equals("asc")
                ? parentComments.OrderBy(c => c.User.UserName)
                : parentComments.OrderByDescending(c => c.User.UserName),

            "email" => sortOrder.Equals("asc")
                ? parentComments.OrderBy(c => c.User.Email)
                : parentComments.OrderByDescending(c => c.User.Email),

            "date" => sortOrder.Equals("asc")
                ? parentComments.OrderBy(c => c.CreatedAt)
                : parentComments.OrderByDescending(c => c.CreatedAt),
            _ => parentComments.OrderByDescending(c => c.CreatedAt)
        };


        var commentsToReturn = await parentComments
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        foreach (var comment in commentsToReturn)
        {
            comment.ChildComments = GetCommentsWithLimitedDepth(comment.CommentId);
        }

        return _mapper.Map<IEnumerable<CommentResponseModel>>(commentsToReturn);
    }

    /// <summary>
    /// Use for adding reply to comment
    /// </summary>
    /// <param name="model"></param>
    /// <exception cref="Exception"></exception>
    // public async Task AddReplyAsync(CommentReplyModel model)
    // {
    //     //TODO: add validation on model for user creation
    //     var user = await _context.User.FirstOrDefaultAsync(u => u.Email == model.UserEmail);
    //     if (user is null)
    //     {
    //         user = new User
    //         {
    //             Email = model.UserEmail,
    //             UserName = model.UserName,
    //             CreatedAt = DateTime.UtcNow
    //         };
    //         await _context.User.AddAsync(user);
    //         await _context.SaveChangesAsync();
    //     }
    //
    //     if (user.UserName != model.UserName)
    //     {
    //         user.UserName = model.UserName;
    //         _context.User.Update(user);
    //         await _context.SaveChangesAsync();
    //     }
    //
    //     var parentComment = await _context.Comment.FindAsync(model.ParentCommentId)
    //                         ?? throw new Exception("Parent comment not found");
    //     parentComment.ChildComments ??= new List<Comment>();
    //     parentComment.ChildComments.Add(new Comment
    //     {
    //         Text = model.Text,
    //         User = user,
    //         CreatedAt = DateTime.UtcNow,
    //         SelectedRandges = model.SelectedRanges.Select(range => new SelectedRange
    //         {
    //             StartIndex = range.StartIndex,
    //             EndIndex = range.EndIndex
    //         }).ToList()
    //     });
    //     _context.Comment.Update(parentComment);
    //     await _context.SaveChangesAsync();
    // }

    public async Task<Comment> GetCommentByIdAsync(int id)
    {
        return await _context.Comment
                   .Include(c => c.User)
                   .Include(c => c.Files)
                   .Include(c => c.ChildComments)
                   .FirstOrDefaultAsync(c => c.CommentId == id)
               ?? throw new Exception("Comment not found");
    }

    /// <summary>
    /// Use for adding comment
    /// </summary>
    /// <param name="model"></param>
    /// <param name="files"></param>
    /// <exception cref="Exception"></exception>
    public async Task AddCommentAsync(CommentCreationModel model, ICollection<IFormFile>? files)
    {
        //TODO: add validation on model for user creation
        var user = await _context.User.FirstOrDefaultAsync(u => u.Email == model.UserEmail);
        if (user is null)
        {
            user = new User
            {
                Email = model.UserEmail,
                UserName = model.UserName,
                CreatedAt = DateTime.UtcNow
            };
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        if (user.UserName != model.UserName)
        {
            user.UserName = model.UserName;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
        }


        //reply logic
        // if (model.ParentCommentId is not null)
        // {
        //     var parentComment = await _context.Comment.FindAsync(model.ParentCommentId)
        //                         ?? throw new Exception("Parent comment not found");
        //     parentComment.ChildComments ??= new List<Comment>();
        //     if (model.StartIndex is not null && model.EndIndex is not null)
        //     {
        //         parentComment.ChildComments.Add(new Comment
        //         {
        //             Text = model.Text,
        //             User = user,
        //             CreatedAt = DateTime.UtcNow,
        //         });
        //         _context.Comment.Update(parentComment);
        //         await _context.SaveChangesAsync();
        //         return;
        //     }
        //
        //     parentComment.ChildComments.Add(new Comment
        //     {
        //         Text = model.Text,
        //         User = user,
        //         CreatedAt = DateTime.UtcNow,
        //
        //     });


        var comment = new Comment
        {
            Text = model.Text,
            ParentCommentId = model.ParentCommentId,
            User = user,
            CreatedAt = DateTime.UtcNow,
        };

        if (model.SelectedRanges is not null)
        {
            comment.SelectedRandges = model.SelectedRanges.Select(range => new SelectedRange
            {
                StartIndex = range.StartIndex,
                EndIndex = range.EndIndex
            }).ToList();
        }

        await _context.Comment.AddAsync(comment);
        await _context.SaveChangesAsync();

        if (files is not null)
        {
            comment.Files = new List<File>();
            foreach (var file in files)
            {
                await _fileService.SaveFileAsync(file, comment.CommentId);
            }
        }

    }

    public async Task DeleteCommentAsync(int id)
    {
        var comment = await _context.Comment.FindAsync(id)
                      ?? throw new Exception("Comment not found");
        _context.Comment.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCommentAsync(CommentUpdateModel model)
    {
        var comment = await _context.Comment.FindAsync(model.CommentId)
                      ?? throw new Exception("Comment not found");
        comment.Text = model.Text;
        _context.Comment.Update(comment);
        await _context.SaveChangesAsync();
    }

    private List<Comment> GetCommentsWithLimitedDepth(int? parentCommentId = null, int currentDepth = 0,
        int maxDepth = 10)
    {
        if (currentDepth >= maxDepth) return new List<Comment>(); // Stop recursion if max depth is reached

        var comments = _context.Comment
            .Where(c => c.ParentCommentId == parentCommentId)
            .Include(c => c.ChildComments)
            .Include(c => c.User)
            .Include(c => c.Files)
            .ToList();

        foreach (var comment in comments)
        {
            comment.ChildComments =
                GetCommentsWithLimitedDepth(comment.CommentId, currentDepth + 1,
                    maxDepth); // Recursively load child replies
        }

        return comments;
    }
}