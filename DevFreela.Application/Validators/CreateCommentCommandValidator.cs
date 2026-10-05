using FluentValidation;
using DevFreela.Application.Commands.CreateComment;
using System.Text.RegularExpressions;
namespace DevFreela.Application.Validators
{
  public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
  {
    public CreateCommentCommandValidator()
    {
      RuleFor(c => c.Content)
        .NotEmpty()
        .WithMessage("Comment is required")
        .MaximumLength(255)
        .WithMessage("Maximum length for a comment is 255 characters");

      RuleFor(c => c.IdUser)
        .GreaterThan(0)
        .WithMessage("User is required");
    }
  }
}