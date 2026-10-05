using FluentValidation;
using DevFreela.Application.Commands.CreateUser;
using System.Text.RegularExpressions;

namespace DevFreela.Application.Validators
{
  public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
  {
    public CreateUserCommandValidator()
    {
      RuleFor(p => p.Email)
          .NotEmpty()
          .WithMessage("E-mail is required")
          .EmailAddress()
          .WithMessage("Wrong e-mail");

      RuleFor(p => p.Password)
          .Must(ValidPassword)
          .WithMessage("Password should have 8 characters, 1 upper, 1 lower and 1 special characters");

      // NotEmpty also rejects null; WithMessage only applies to the rule right before it
      RuleFor(p => p.FullName)
            .NotEmpty()
            .WithMessage("Name is required");

      RuleFor(p => p.BirthDate)
            .LessThan(_ => DateTime.Today)
            .WithMessage("Birth date must be in the past");

      // The JWT role claim and the [Authorize(Roles = ...)] checks only know these two
      RuleFor(p => p.Role)
            .Must(role => role == "client" || role == "freelancer")
            .WithMessage("Role must be client or freelancer");
    }
    public bool ValidPassword(string password)
    {
      if (string.IsNullOrEmpty(password))
      {
        return false;
      }

      var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");

      return regex.IsMatch(password);
    }
  }
}