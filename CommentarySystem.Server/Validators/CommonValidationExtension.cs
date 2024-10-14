using FluentValidation;

namespace CommentarySystem.Server.Validators;

public static class CommonValidationExtension
{
    public static IRuleBuilderOptions<T, string> StringValidation<T>(this IRuleBuilder<T, string> ruleBuilder,
        int maxLength, int minLength = 0, string stringName = "")
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage($"{stringName} is required")
            .MaximumLength(maxLength)
            .WithMessage($"{stringName} must be less than {maxLength} characters")
            .MinimumLength(minLength)
            .WithMessage($"{stringName} must be more than {minLength} characters");
    }
}

