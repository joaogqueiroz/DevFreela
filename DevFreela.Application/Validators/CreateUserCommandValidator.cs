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
          .EmailAddress()
          .WithMessage("Wrong e-mail");

      RuleFor(p => p.Password)
          .Must(ValidPassword)
          .WithMessage("Password should have 8 characters, 1 upper, 1 lower and 1 special characters");

      RuleFor(p => p.FullName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name is required");
    }
    public bool ValidPassword(string password)
    {
      var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");

      return regex.IsMatch(password);
    }
  }
}