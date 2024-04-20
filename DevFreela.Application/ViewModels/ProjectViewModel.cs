using System;
namespace DevFreela.Application.ViewModels
{
  public class ProjectViewModel
  {
    public ProjectViewModel(int id, string title, string description, decimal? totalCost,DateTime createdAt)
    {
      Id = id;
      Title = title;
      Description = description;
      TotalCost = totalCost;
      CreatedAt = createdAt;
    }

    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public decimal? TotalCost { get; private set; }
    public DateTime CreatedAt { get; private set; }
  }
}