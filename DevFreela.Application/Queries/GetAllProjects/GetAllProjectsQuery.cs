using System;
using System.Collections.Generic;
using MediatR;
using DevFreela.Application.ViewModels;
namespace DevFreela.Application.Queries.GetAllProjects
{
  public class GetAllProjectsQuery : IRequest<List<ProjectViewModel>>
  {
    public GetAllProjectsQuery(string query)
    {
      Query = query;
    }
    public string Query { get; private set; }
  }
}