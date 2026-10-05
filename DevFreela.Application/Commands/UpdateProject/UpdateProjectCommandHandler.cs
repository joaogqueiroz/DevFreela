using MediatR;
using DevFreela.Core.Repositories;
namespace DevFreela.Application.Commands.UpdateProject
{
  public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
  {
    private readonly IProjectRepository _projectRepository;
    public UpdateProjectCommandHandler(IProjectRepository projectRepository)
    {
      _projectRepository = projectRepository;

    }

    public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
      var project = await _projectRepository.GetByIdAsync(request.Id);
      if (project == null)
      {
        return false;
      }
      project.Update(request.Title, request.Description, request.TotalCost);
      await _projectRepository.SaveChangesAsync();
      return true;
    }
  }
}