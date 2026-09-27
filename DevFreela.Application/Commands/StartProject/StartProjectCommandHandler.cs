using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Core.Repositories;
namespace DevFreela.Application.Commands.StartProject
{
  public class StartProjectCommandHandler : IRequestHandler<StartProjectCommand, Unit>
  {
    private readonly IProjectRepository _projectRepository;
    public StartProjectCommandHandler(IProjectRepository projectRepository)
    {
      _projectRepository = projectRepository;
    }
    public async Task<Unit> Handle(StartProjectCommand request, CancellationToken cancellationToken)
    {
      var project = await _projectRepository.GetByIdAsync(request.Id);
      project.Start();
      await _projectRepository.SaveChangesAsync();
      return Unit.Value;
    }
  }
}