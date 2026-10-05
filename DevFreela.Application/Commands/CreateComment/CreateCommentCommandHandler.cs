using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
namespace DevFreela.Application.Commands.CreateComment
{
  public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, bool>
  {

    private readonly IProjectRepository _projectRepository;
    public CreateCommentCommandHandler(IProjectRepository projectRepository)
    {
      _projectRepository = projectRepository;
    }
    public async Task<bool> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
      var project = await _projectRepository.GetByIdAsync(request.IdProject);
      if (project == null)
      {
        return false;
      }
      var comment = new ProjectComment(request.Content, request.IdProject, request.IdUser);
      await _projectRepository.AddCommentAsync(comment);
      return true;
    }
  }
}