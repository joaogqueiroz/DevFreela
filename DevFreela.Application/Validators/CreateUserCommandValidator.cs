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

            RuleFor(p => p.PassWord)
                .Must(ValidPassword)
                .WithMessage("Password should have ...");
    }
        public bool ValidPassword(string password)
        {
            var regex = new Regex(@"");

            return regex.IsMatch(password);
        }
  }
}