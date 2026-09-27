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
      .MaximumLength(255)
      .WithMessage("Maximum length for description is 255 characters");
    }
  }
}