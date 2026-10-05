using MediatR;
namespace DevFreela.Application.Commands.DeleteProject
{
  public class DeleteProjectCommand : IRequest<bool>
  {
    public DeleteProjectCommand(int id)
    {
      Id = id;
    }

    public int Id { get; private set; }
  }
}