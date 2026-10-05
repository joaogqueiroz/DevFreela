using Moq;
using DevFreela.Core.DTOs;
using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using DevFreela.Application.Commands.FinishProject;
using DevFreela.Application.Commands.StartProject;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
namespace DevFreela.UnitTests.Application.Commands
{
  public class ProjectLifecycleCommandHandlerTests
  {
    private static Project StartedProject()
    {
      var project = new Project("Project title", "Project description", 1, 2, 1500);
      project.Start();
      return project;
    }

    [Fact]
    public async Task StartProject_SetsInProgressAndSaves()
    {
      //Arrange
      var project = new Project("Project title", "Project description", 1, 2, 1500);
      var projectRepositoryMock = new Mock<IProjectRepository>();
      projectRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);
      var handler = new StartProjectCommandHandler(projectRepositoryMock.Object);

      //Act
      await handler.Handle(new StartProjectCommand(1), new CancellationToken());

      //Assert
      Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
      Assert.NotNull(project.StartedAt);
      projectRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task FinishProject_SavesPaymentPendingBeforeRequestingPayment()
    {
      //Arrange
      var project = StartedProject();
      var steps = new List<string>();
      var projectRepositoryMock = new Mock<IProjectRepository>();
      projectRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);
      projectRepositoryMock
        .Setup(r => r.SaveChangesAsync())
        .Callback(() => steps.Add($"save:{project.Status}"))
        .Returns(Task.CompletedTask);
      var paymentServiceMock = new Mock<IPaymentService>();
      paymentServiceMock
        .Setup(p => p.ProcessPayment(It.IsAny<PaymentInfoDTO>()))
        .Callback(() => steps.Add("payment"));
      var handler = new FinishProjectCommandHandler(projectRepositoryMock.Object, paymentServiceMock.Object);

      //Act
      await handler.Handle(new FinishProjectCommand { Id = 1, CreditCardNumber = "4111111111111111", Cvv = "123", ExpiresAt = "12/30", FullName = "Client" }, new CancellationToken());

      //Assert
      // The payment-approved consumer finishes only PaymentPending projects, so the status
      // has to be in the database before the payment request can be answered.
      Assert.Equal(new[] { $"save:{ProjectStatusEnum.PaymentPending}", "payment" }, steps);
    }

    [Fact]
    public async Task FinishProject_SendsThePaymentDetailsAndTheProjectCost()
    {
      //Arrange
      var projectRepositoryMock = new Mock<IProjectRepository>();
      projectRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(StartedProject());
      var paymentServiceMock = new Mock<IPaymentService>();
      var handler = new FinishProjectCommandHandler(projectRepositoryMock.Object, paymentServiceMock.Object);

      //Act
      await handler.Handle(new FinishProjectCommand { Id = 1, CreditCardNumber = "4111111111111111", Cvv = "123", ExpiresAt = "12/30", FullName = "Client" }, new CancellationToken());

      //Assert
      paymentServiceMock.Verify(p => p.ProcessPayment(It.Is<PaymentInfoDTO>(dto =>
        dto.IdProject == 1 &&
        dto.CreditCardNumber == "4111111111111111" &&
        dto.Cvv == "123" &&
        dto.ExpiresAt == "12/30" &&
        dto.FullName == "Client" &&
        dto.Amount == 1500)), Times.Once);
    }

    private static Project ProjectIn(string state)
    {
      var project = new Project("Project title", "Project description", 1, 2, 1500);
      switch (state)
      {
        case "Created":
          break;
        case "PaymentPending":   // already finished once, waiting for the payment
          project.Start();
          project.SetPaymentPending();
          break;
        case "Finished":
          project.Start();
          project.SetPaymentPending();
          project.Finish();
          break;
        case "Cancelled":
          project.Start();
          project.Cancel();
          break;
      }
      Assert.Equal(state, project.Status.ToString());
      return project;
    }

    [Theory]
    [InlineData("Created")]          // never started: nothing to pay for yet
    [InlineData("PaymentPending")]   // finishing twice would charge twice
    [InlineData("Finished")]
    [InlineData("Cancelled")]
    public async Task FinishProject_NotInProgress_IsRefusedWithoutPayment(string state)
    {
      //Arrange
      var project = ProjectIn(state);
      var projectRepositoryMock = new Mock<IProjectRepository>();
      projectRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);
      var paymentServiceMock = new Mock<IPaymentService>();
      var handler = new FinishProjectCommandHandler(projectRepositoryMock.Object, paymentServiceMock.Object);

      //Act
      var result = await handler.Handle(new FinishProjectCommand { Id = 1, CreditCardNumber = "4111111111111111", Cvv = "123", ExpiresAt = "12/30", FullName = "Client" }, new CancellationToken());

      //Assert
      Assert.Equal(FinishProjectResult.ProjectNotInProgress, result);
      Assert.Equal(state, project.Status.ToString());
      paymentServiceMock.Verify(p => p.ProcessPayment(It.IsAny<PaymentInfoDTO>()), Times.Never);
      projectRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task FinishProject_InProgress_RequestsPayment()
    {
      //Arrange
      var projectRepositoryMock = new Mock<IProjectRepository>();
      projectRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(StartedProject());
      var handler = new FinishProjectCommandHandler(projectRepositoryMock.Object, new Mock<IPaymentService>().Object);

      //Act
      var result = await handler.Handle(new FinishProjectCommand { Id = 1, CreditCardNumber = "4111111111111111", Cvv = "123", ExpiresAt = "12/30", FullName = "Client" }, new CancellationToken());

      //Assert
      Assert.Equal(FinishProjectResult.PaymentRequested, result);
    }
  }
}
