using FluentValidation;
using DevFreela.Application.Commands.UpdateProject;
using System.Text.RegularExpressions;
namespace DevFreela.Application.Validators
{
  public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
  {
    public UpdateProjectCommandValidator()
    {
      RuleFor(p => p.Title)
        .NotEmpty()
        .WithMessage("Title is required")
        .MaximumLength(30)
        .WithMessage("Maximum length for title is 30 characters");

      RuleFor(p => p.Description)
        .NotEmpty()
        .WithMessage("Description is required")
        .MaximumLength(255)
        .WithMessage("Maximum length for description is 255 characters");

      // The total cost is the amount charged when the project is finished
      RuleFor(p => p.TotalCost)
        .NotNull()
        .WithMessage("Total cost is required")
        .GreaterThan(0)
        .WithMessage("Total cost must be greater than zero");
    }
  }
}