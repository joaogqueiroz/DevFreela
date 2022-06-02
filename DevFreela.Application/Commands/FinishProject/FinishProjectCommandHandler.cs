using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Core.Repositories;
namespace DevFreela.Application.Commands.FinishProject
{
  public class FinishProjectCommandHandler : IRequestHandler<FinishProjectCommand, Unit>
  {
    private readonly IProjectRepository _projectRepository;
    public FinishProjectCommandHandler(IProjectRepository projectRepository)
    {
      _projectRepository = projectRepository;
    }
    public async Task<Unit> Handle(FinishProjectCommand request, CancellationToken cancellationToken)
    {
      var project = await _projectRepository.GetByIdAsync(request.Id);
      project.Finish();
      await _projectRepository.SaveChangesAsync();
      return Unit.Value;
    }
  }
}