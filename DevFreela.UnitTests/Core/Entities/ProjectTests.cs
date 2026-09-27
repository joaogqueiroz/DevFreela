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

    [Fact]
    public void TestIfProjectFinishesAfterPaymentIsApproved()
    {
      var project = new Project("Project Title", "Project Description", 1, 2, 1000);
      project.Start();
      project.SetPaymentPending();

      Assert.Equal(ProjectStatusEnum.PaymentPending, project.Status);
      Assert.Null(project.FinishedAt);

      project.Finish();

      Assert.Equal(ProjectStatusEnum.Finished, project.Status);
      Assert.NotNull(project.FinishedAt);
    }

    [Fact]
    public void TestIfProjectDoesNotFinishWithoutPendingPayment()
    {
      var project = new Project("Project Title", "Project Description", 1, 2, 1000);
      project.Start();

      project.Finish();

      Assert.Equal(ProjectStatusEnum.InProgress, project.Status);
      Assert.Null(project.FinishedAt);
    }
  }
}