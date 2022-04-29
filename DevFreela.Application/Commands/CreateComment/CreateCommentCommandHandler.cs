using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
namespace DevFreela.Application.Commands.CreateComment
{
  public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Unit>
  {

    private readonly DevFreelaDbContext _dbContext;
    public CreateCommentCommandHandler(DevFreelaDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<Unit> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
      var comment = new ProjectComment(request.Content, request.IdProject, request.IdUser);
      await _dbContext.ProjectComments.AddAsync(comment);
      await _dbContext.SaveChangesAsync();
      return Unit.Value;
    }
  }
}