using DevFreela.Application.Commands.CreateUser;
using DevFreela.Application.Validators;
using FluentValidation.TestHelper;
using System;
using Xunit;
namespace DevFreela.UnitTests.Application.Validators
{
  public class CreateUserCommandValidatorTests
  {
    private readonly CreateUserCommandValidator _validator = new CreateUserCommandValidator();

    private static CreateUserCommand ValidCommand() => new CreateUserCommand
    {
      FullName = "Client User",
      Email = "client@test.com",
      Password = "Senha@123",
      BirthDate = new DateTime(1990, 1, 1),
      Role = "client"
    };

    [Theory]
    [InlineData("client")]
    [InlineData("freelancer")]
    public void ValidCommand_HasNoErrors(string role)
    {
      var command = ValidCommand();
      command.Role = role;

      _validator.TestValidate(command).ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("", "E-mail is required")]
    [InlineData(null, "E-mail is required")]
    [InlineData("not-an-email", "Wrong e-mail")]
    public void MissingOrWrongEmail_IsRejected(string email, string message)
    {
      var command = ValidCommand();
      command.Email = email;

      _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.Email).WithErrorMessage(message);
    }

    [Theory]
    [InlineData(null)]          // used to throw inside the regex
    [InlineData("")]
    [InlineData("Se@1")]        // too short
    [InlineData("senha@123")]   // no uppercase
    [InlineData("SENHA@123")]   // no lowercase
    [InlineData("Senha@abc")]   // no digit
    [InlineData("Senha1234")]   // no special character
    public void MissingOrWeakPassword_IsRejected(string password)
    {
      var command = ValidCommand();
      command.Password = password;

      _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.Password);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void MissingName_UsesTheCustomMessage(string name)
    {
      var command = ValidCommand();
      command.FullName = name;

      _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.FullName).WithErrorMessage("Name is required");
    }

    [Fact]
    public void BirthDateTodayOrInTheFuture_IsRejected()
    {
      var today = ValidCommand();
      today.BirthDate = DateTime.Today;
      var future = ValidCommand();
      future.BirthDate = DateTime.Today.AddYears(1);

      _validator.TestValidate(today).ShouldHaveValidationErrorFor(c => c.BirthDate);
      _validator.TestValidate(future).ShouldHaveValidationErrorFor(c => c.BirthDate);
    }

    [Theory]
    [InlineData("admin")]
    [InlineData("Client")]      // roles are case-sensitive in [Authorize(Roles = ...)]
    [InlineData("")]
    [InlineData(null)]
    public void UnknownRole_IsRejected(string role)
    {
      var command = ValidCommand();
      command.Role = role;

      _validator.TestValidate(command).ShouldHaveValidationErrorFor(c => c.Role).WithErrorMessage("Role must be client or freelancer");
    }
  }
}
