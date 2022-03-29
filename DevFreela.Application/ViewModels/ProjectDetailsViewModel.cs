using System;
namespace DevFreela.Application.ViewModels
{
  public class ProjectDetailsViewModel
  {
    public ProjectDetailsViewModel(int id, string title, string description, decimal? totalCoast, DateTime? startedAt, DateTime? finishedAt)
    {
      Id = id;
      Title = title;
      Description = description;
      TotalCoast = totalCoast;
      StartedAt = startedAt;
      FinishedAt = finishedAt;
    }

    public int Id { get; private set; }
    public String Title { get; private set; }
    public string Description { get; private set; }
    public decimal? TotalCoast { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }


  }
}