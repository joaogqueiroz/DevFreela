using FluentValidation;
using DevFreela.Application.Commands.CreateProject;
using System.Text.RegularExpressions;
namespace DevFreela.Application.Validators
{
  public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
  {
    public CreateProjectCommandValidator()
    {
      RuleFor(p => p.Description)
      .MaximumLength(255)
      .WithMessage("Maximum length for description is 255 characters");

      RuleFor(p => p.Title)
      .MaximumLength(30)
      .WithMessage("Maximum length for title is 30 characters");
    }
  }
}