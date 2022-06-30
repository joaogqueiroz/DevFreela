using DevFreela.Core.Entities;
using DevFreela.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace DevFreela.UnitTests.Core.Entities
{
  public class ProjectTests
  {
    [Fact]
    public void TestIfProjectStartToWorks()
    {
      var project = new Project("Project Title", "Project Description", 1, 2, 1000);

      Assert.Equal(ProjectStatusEnum.Created, project.Status);
      Assert.Null(project.StartedAt);

      Assert.NotNull(project.Title);
      Assert.NotEmpty(project.Title);

      Assert.NotNull(project.Description);
      Assert.NotEmpty(project.Description);

      project.Start();

      Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
      Assert.NotNull(project.StartedAt);

    }
  }
}