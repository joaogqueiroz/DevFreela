namespace DevFreela.Application.ViewModels
{
  public class SkillViewModel
  {
    public SkillViewModel(int id, string description)
    {
      this.Id = id;
      this.Description = description;

    }
    public int Id { get; private set; }
    public string Description { get; private set; }


  }
}