using Moq;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Xunit;
namespace DevFreela.UnitTests.Application.Queries
{
  public class GetAllProjectCommandHandlerTests
  {
    [Fact]
    public async Task ThreeProjectsExists_Executed_ReturnThreeProjectsViewModels()
    {
      //Arrange
      var projects = new List<Project>
      {
        new Project("Nome projeto 1", "Descricao projeto 1", 1,2,1000),
        new Project("Nome projeto 2", "Descricao projeto 1", 1,2,2000),
        new Project("Nome projeto 3", "Descricao projeto 1", 1,2,3000)
      };
      var projectRepositoryMock = new Mock<IProjectRepository>();
      projectRepositoryMock.Setup(pr => pr.GetAllAsync().Result).Returns(projects);

      var getAllProjectsQuery = new GetAllProjectsQuery("");
      var getAllProjectsQueryHandler = new GetAllProjectsQueryHandler(projectRepositoryMock.Object);
      //Act
      var projectViewModelList = await getAllProjectsQueryHandler.Handle(getAllProjectsQuery, new CancellationToken());


      //Assert
      Assert.NotNull(projectViewModelList);
      Assert.NotEmpty(projectViewModelList);
      Assert.Equal(projects.Count, projectViewModelList.Count);
      projectRepositoryMock.Verify(pr => pr.GetAllAsync().Result, Times.Once);


    }
  }
}