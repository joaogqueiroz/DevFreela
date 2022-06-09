using FluentValidation;
using DevFreela.Application.Commands.UpdateProject;
using System.Text.RegularExpressions;
namespace DevFreela.Application.Validators
{
  public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
  {
    public UpdateProjectCommandValidator()
    {
      RuleFor(p => p.Description)
                .MaximumLength(255)
                .WithMessage("Maximum length for description is 30 characters");

      RuleFor(p => p.Title)
          .MaximumLength(30)
          .WithMessage("Maximum length for title is 30 characters");
    }
  }
}