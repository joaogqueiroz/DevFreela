using Moq;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Xunit;
namespace DevFreela.UnitTests.Application.Commands
{
  public class CreateProjectCommandHandlerTests
  {
    [Fact]
    public async Task InputDateIsOk_Return_ProjectId()
    {
      //Arrange
      var projectRepositoryMock = new Mock<IProjectRepository>();
      var createProjectCommand = new CreateProjectCommand
      {
        Title = "Project title",
        Description = "Project description",
        TotalCost = 1000,
        IdClient = 1,
        IdFreelancer = 2
      };
      var createProjectCommandHandler = new CreateProjectCommandHandler(projectRepositoryMock.Object);

      //Act
      var id = await createProjectCommandHandler.Handle(createProjectCommand, new CancellationToken());

      //Assert
      Assert.True(id >= 0);
      projectRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Project>()), Times.Once);

    }
  }
}