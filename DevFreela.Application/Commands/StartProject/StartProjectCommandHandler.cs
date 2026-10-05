using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Core.Repositories;
namespace DevFreela.Application.Commands.StartProject
{
  public class StartProjectCommandHandler : IRequestHandler<StartProjectCommand, bool>
  {
    private readonly IProjectRepository _projectRepository;
    public StartProjectCommandHandler(IProjectRepository projectRepository)
    {
      _projectRepository = projectRepository;
    }
    public async Task<bool> Handle(StartProjectCommand request, CancellationToken cancellationToken)
    {
      var project = await _projectRepository.GetByIdAsync(request.Id);
      if (project == null)
      {
        return false;
      }
      project.Start();
      await _projectRepository.SaveChangesAsync();
      return true;
    }
  }
}