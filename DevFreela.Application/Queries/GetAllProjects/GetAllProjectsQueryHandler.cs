using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Repositories;
namespace DevFreela.Application.Queries.GetAllProjects
{
  public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ProjectViewModel>>
  {
    private readonly IProjectRepository _projectRepository;
    public GetAllProjectsQueryHandler(IProjectRepository projectRepository)
    {
      _projectRepository = projectRepository;
    }
    public async Task<List<ProjectViewModel>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
      var projects = await _projectRepository.GetAllAsync();
      var projectsViewModel = projects
      .Select(p => new ProjectViewModel(p.Id, p.Title, p.Description, p.TotalCost, p.CreatedAt))
      .ToList();
      return projectsViewModel;
    }
  }
}