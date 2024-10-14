using CommentarySystem.Server.Model;
using FluentValidation;

namespace CommentarySystem.Server.Validators;

public class CommentCreateValidation : AbstractValidator<CommentCreationModel>
{
    public CommentCreateValidation()
    {
        RuleFor(x => x.Text).NotEmpty().WithMessage("Text is required");
        RuleFor(x => x.Text).MaximumLength(100_000).WithMessage("Text is too long");
        RuleFor(x => x.UserEmail).NotEmpty().WithMessage("User email is required");
        RuleFor(x => x.UserEmail).EmailAddress().WithMessage("User email is not valid");
        RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
        RuleFor(x => x.UserName).MaximumLength(100).WithMessage("User name is too long");
    }
}