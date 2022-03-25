using System;
using System.Collections.Generic;
using DevFreela.Core.Enums;
namespace DevFreela.Core.Entities
{
  public class Project : BaseEntity
  {
    public Project(int title, int description, int idClient, int idFreelancer, decimal totalCost)
    {
      Title = title;
      Description = description;
      IdClient = idClient;
      IdFreelancer = idFreelancer;
      TotalCost = totalCost;
      CreatedAt = DateTime.Now;
      Status = ProjectStatusEnum.Created;
      Comments = new List<ProjectComment>();

    }
    public int Title { get; private set; }
    public int Description { get; private set; }
    public int IdClient { get; private set; }
    public int IdFreelancer { get; private set; }
    public decimal TotalCost { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public ProjectStatusEnum Status { get; private set; }
    public List<ProjectComment> Comments { get; private set; }
  }
}