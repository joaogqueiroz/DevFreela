using System;
namespace DevFreela.Application.ViewModels
{
  public class ProjectDetailsViewModel
  {
    public ProjectDetailsViewModel(int id, string title, string description, decimal? totalCoast, DateTime? startedAt, DateTime? finishedAt, string clientFullName, string freelanceFullName)
    {
      Id = id;
      Title = title;
      Description = description;
      TotalCoast = totalCoast;
      StartedAt = startedAt;
      FinishedAt = finishedAt;
      ClientFullName = clientFullName;
      FreelanceFullName = freelanceFullName;
    }

    public int Id { get; private set; }
    public String Title { get; private set; }
    public string Description { get; private set; }
    public decimal? TotalCoast { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public string ClientFullName { get; private set; }
    public string FreelanceFullName { get; private set; }


  }
}