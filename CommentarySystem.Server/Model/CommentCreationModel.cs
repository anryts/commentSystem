namespace CommentarySystem.Server.Model;

/// <summary>
/// Model which represents the data needed to create a comment.
/// </summary>
/// <param name="Text">Can be a html and must be not larger than 100KB</param>
/// <param name="ParentCommentId">If null, that's mean it's parent</param>
public record CommentCreationModel(
    string UserName,
    string UserEmail,
    int? ParentCommentId,
    string Text,
    IEnumerable<SelectedRangeModel>? SelectedRanges);

/// <summary>
/// Model which represents the data needed to update a comment.
/// </summary>
/// <param name="CommentId"></param>
/// <param name="Text">Again modified text must not be larger than 100KB</param>
public record CommentUpdateModel(int CommentId, string Text);


public record SelectedRangeModel(int StartIndex, int EndIndex);