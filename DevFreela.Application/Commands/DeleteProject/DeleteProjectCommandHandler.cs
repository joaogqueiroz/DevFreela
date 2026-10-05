using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Repositories;
namespace DevFreela.Application.Commands.DeleteProject
{
  public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
  {
    private readonly IProjectRepository _projectRepository;
    public DeleteProjectCommandHandler(IProjectRepository projectRepository)
    {
      _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
      var project = await _projectRepository.GetByIdAsync(request.Id);
      if (project == null)
      {
        return false;
      }
      project.Cancel();
      await _projectRepository.SaveChangesAsync();
      return true;
    }
  }
}