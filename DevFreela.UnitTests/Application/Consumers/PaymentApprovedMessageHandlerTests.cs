using Moq;
using DevFreela.Application.Consumers;
using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using DevFreela.Core.Repositories;
using System.Threading.Tasks;
using Xunit;
namespace DevFreela.UnitTests.Application.Consumers
{
  public class PaymentApprovedMessageHandlerTests
  {
    private static Project PaymentPendingProject()
    {
      var project = new Project("Project title", "Project description", 1, 2, 1500);
      project.Start();
      project.SetPaymentPending();
      return project;
    }

    [Fact]
    public async Task ApprovedPayment_FinishesTheProject()
    {
      //Arrange
      var project = PaymentPendingProject();
      var projectRepositoryMock = new Mock<IProjectRepository>();
      projectRepositoryMock.Setup(r => r.GetByIdAsync(41)).ReturnsAsync(project);
      var handler = new PaymentApprovedMessageHandler(projectRepositoryMock.Object);

      //Act
      var handled = await handler.HandleAsync(@"{""IdProject"":41}");

      //Assert
      Assert.True(handled);
      Assert.Equal(ProjectStatusEnum.Finished, project.Status);
      projectRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData("this is not json")]
    [InlineData("null")]
    [InlineData(@"{""IdProject"":""41""}")]
    [InlineData(@"{""idProject"":41}")]
    [InlineData(@"{""IdProject"":0}")]
    [InlineData("{}")]
    public async Task InvalidMessage_IsDroppedWithoutTouchingProjects(string message)
    {
      //Arrange
      var projectRepositoryMock = new Mock<IProjectRepository>();
      var handler = new PaymentApprovedMessageHandler(projectRepositoryMock.Object);

      //Act
      var handled = await handler.HandleAsync(message);

      //Assert
      Assert.False(handled);
      projectRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
      projectRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UnknownProject_IsDropped()
    {
      //Arrange
      var projectRepositoryMock = new Mock<IProjectRepository>();
      var handler = new PaymentApprovedMessageHandler(projectRepositoryMock.Object);

      //Act
      var handled = await handler.HandleAsync(@"{""IdProject"":999}");

      //Assert
      Assert.False(handled);
      projectRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
  }
}
