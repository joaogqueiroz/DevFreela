using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
namespace DevFreela.Application.Commands.CreateProject
{
  public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
  {
    private readonly DevFreelaDbContext _dbContext;
    public CreateProjectCommandHandler(DevFreelaDbContext dbContext)
    {
      _dbContext = dbContext;

    }
    public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
      var project = new Project(request.Title, request.Description, request.IdClient, request.IdFreelancer, request.TotalCost);
      await _dbContext.Projects.AddAsync(project);
      await _dbContext.SaveChangesAsync();
      return project.Id;
    }
  }
}