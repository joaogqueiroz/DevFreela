using DevFreela.Application.Commands.CreateComment;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.Validators;
using FluentValidation.TestHelper;
using Xunit;
namespace DevFreela.UnitTests.Application.Validators
{
  public class ProjectCommandValidatorTests
  {
    private readonly CreateProjectCommandValidator _createValidator = new CreateProjectCommandValidator();
    private readonly UpdateProjectCommandValidator _updateValidator = new UpdateProjectCommandValidator();
    private readonly CreateCommentCommandValidator _commentValidator = new CreateCommentCommandValidator();

    private static CreateProjectCommand ValidProject() => new CreateProjectCommand
    {
      Title = "Landing page",
      Description = "Build a landing page for the product launch",
      IdClient = 1,
      IdFreelancer = null,
      TotalCost = 1500
    };

    [Fact]
    public void ValidProject_HasNoErrors()
    {
      _createValidator.TestValidate(ValidProject()).ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void TitleAndDescriptionAtTheirLimits_AreValid()
    {
      var project = ValidProject();
      project.Title = new string('t', 30);
      project.Description = new string('d', 255);

      _createValidator.TestValidate(project).ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("", "Title is required")]
    [InlineData(null, "Title is required")]
    [InlineData("This title is longer than thirty", "Maximum length for title is 30 characters")]
    public void InvalidTitle_IsRejected(string title, string message)
    {
      var project = ValidProject();
      project.Title = title;

      _createValidator.TestValidate(project).ShouldHaveValidationErrorFor(p => p.Title).WithErrorMessage(message);
    }

    [Fact]
    public void DescriptionOverTheLimit_IsRejected()
    {
      var project = ValidProject();
      project.Description = new string('d', 256);

      _createValidator.TestValidate(project).ShouldHaveValidationErrorFor(p => p.Description)
        .WithErrorMessage("Maximum length for description is 255 characters");
    }

    [Fact]
    public void EmptyDescription_IsRejected()
    {
      var project = ValidProject();
      project.Description = "";

      _createValidator.TestValidate(project).ShouldHaveValidationErrorFor(p => p.Description).WithErrorMessage("Description is required");
    }

    // TotalCost becomes the payment amount when the project is finished
    [Theory]
    [InlineData("0", "Total cost must be greater than zero")]
    [InlineData("-100", "Total cost must be greater than zero")]
    [InlineData(null, "Total cost is required")]
    public void ZeroNegativeOrMissingCost_IsRejected(string cost, string message)
    {
      var project = ValidProject();
      project.TotalCost = cost == null ? null : decimal.Parse(cost);

      _createValidator.TestValidate(project).ShouldHaveValidationErrorFor(p => p.TotalCost).WithErrorMessage(message);
    }

    [Fact]
    public void SmallestPositiveCost_IsValid()
    {
      var project = ValidProject();
      project.TotalCost = 0.01m;

      _createValidator.TestValidate(project).ShouldNotHaveValidationErrorFor(p => p.TotalCost);
    }

    [Fact]
    public void ProjectWithoutClient_IsRejected()
    {
      var project = ValidProject();
      project.IdClient = 0;

      _createValidator.TestValidate(project).ShouldHaveValidationErrorFor(p => p.IdClient).WithErrorMessage("Client is required");
    }

    [Fact]
    public void Update_DescriptionOverTheLimit_SaysTheRealLimit()
    {
      // The message used to say 30 characters for a 255 limit
      var result = _updateValidator.TestValidate(new UpdateProjectCommand { Id = 1, Title = "Title", Description = new string('d', 256), TotalCost = 100 });

      result.ShouldHaveValidationErrorFor(p => p.Description).WithErrorMessage("Maximum length for description is 255 characters");
    }

    [Fact]
    public void Update_NegativeCost_IsRejected()
    {
      var result = _updateValidator.TestValidate(new UpdateProjectCommand { Id = 1, Title = "Title", Description = "Description", TotalCost = -1 });

      result.ShouldHaveValidationErrorFor(p => p.TotalCost);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void EmptyComment_IsRejected(string content)
    {
      var result = _commentValidator.TestValidate(new CreateCommentCommand { IdProject = 1, IdUser = 1, Content = content });

      result.ShouldHaveValidationErrorFor(c => c.Content).WithErrorMessage("Comment is required");
    }

    [Fact]
    public void CommentOverTheLimit_IsRejected()
    {
      var result = _commentValidator.TestValidate(new CreateCommentCommand { IdProject = 1, IdUser = 1, Content = new string('c', 256) });

      result.ShouldHaveValidationErrorFor(c => c.Content).WithErrorMessage("Maximum length for a comment is 255 characters");
    }

    [Fact]
    public void CommentWithoutUser_IsRejected()
    {
      var result = _commentValidator.TestValidate(new CreateCommentCommand { IdProject = 1, IdUser = 0, Content = "Hello" });

      result.ShouldHaveValidationErrorFor(c => c.IdUser).WithErrorMessage("User is required");
    }
  }
}
