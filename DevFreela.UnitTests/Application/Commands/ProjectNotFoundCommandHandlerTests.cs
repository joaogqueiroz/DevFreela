using Moq;
using DevFreela.Core.DTOs;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using DevFreela.Application.Commands.CreateComment;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.FinishProject;
using DevFreela.Application.Commands.StartProject;
using DevFreela.Application.Commands.UpdateProject;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
namespace DevFreela.UnitTests.Application.Commands
{
  // Every command on a project id that does not exist used to call a method on null (500).
  public class ProjectNotFoundCommandHandlerTests
  {
    private const int UnknownId = 999;

    // GetByIdAsync is not set up, so Moq returns null like the repository does for an unknown id
    private readonly Mock<IProjectRepository> _projectRepositoryMock = new Mock<IProjectRepository>();

    private void VerifyNothingWasSaved() =>
      _projectRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);

    [Fact]
    public async Task StartProject_UnknownId_ReturnsFalse()
    {
      var found = await new StartProjectCommandHandler(_projectRepositoryMock.Object)
        .Handle(new StartProjectCommand(UnknownId), new CancellationToken());

      Assert.False(found);
      VerifyNothingWasSaved();
    }

    [Fact]
    public async Task UpdateProject_UnknownId_ReturnsFalse()
    {
      var found = await new UpdateProjectCommandHandler(_projectRepositoryMock.Object)
        .Handle(new UpdateProjectCommand { Id = UnknownId, Title = "Title", Description = "Description", TotalCost = 100 }, new CancellationToken());

      Assert.False(found);
      VerifyNothingWasSaved();
    }

    [Fact]
    public async Task DeleteProject_UnknownId_ReturnsFalse()
    {
      var found = await new DeleteProjectCommandHandler(_projectRepositoryMock.Object)
        .Handle(new DeleteProjectCommand(UnknownId), new CancellationToken());

      Assert.False(found);
      VerifyNothingWasSaved();
    }

    [Fact]
    public async Task CreateComment_UnknownProject_ReturnsFalseAndSavesNoComment()
    {
      var found = await new CreateCommentCommandHandler(_projectRepositoryMock.Object)
        .Handle(new CreateCommentCommand { IdProject = UnknownId, IdUser = 1, Content = "Hello" }, new CancellationToken());

      Assert.False(found);
      _projectRepositoryMock.Verify(r => r.AddCommentAsync(It.IsAny<ProjectComment>()), Times.Never);
    }

    [Fact]
    public async Task CreateComment_ExistingProject_SavesTheComment()
    {
      _projectRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Project("Title", "Description", 1, 2, 100));

      var found = await new CreateCommentCommandHandler(_projectRepositoryMock.Object)
        .Handle(new CreateCommentCommand { IdProject = 1, IdUser = 1, Content = "Hello" }, new CancellationToken());

      Assert.True(found);
      _projectRepositoryMock.Verify(r => r.AddCommentAsync(It.Is<ProjectComment>(c => c.IdProject == 1 && c.Content == "Hello")), Times.Once);
    }

    [Fact]
    public async Task FinishProject_UnknownId_RequestsNoPayment()
    {
      var paymentServiceMock = new Mock<IPaymentService>();

      var result = await new FinishProjectCommandHandler(_projectRepositoryMock.Object, paymentServiceMock.Object)
        .Handle(new FinishProjectCommand { Id = UnknownId, CreditCardNumber = "4111111111111111", Cvv = "123", ExpiresAt = "12/30", FullName = "Client" }, new CancellationToken());

      Assert.Equal(FinishProjectResult.ProjectNotFound, result);
      paymentServiceMock.Verify(p => p.ProcessPayment(It.IsAny<PaymentInfoDTO>()), Times.Never);
      VerifyNothingWasSaved();
    }
  }
}
