using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace DevFreela.Application.Queries.GetProjectById
{
  public class GetProjectByIdQuery : IRequest<ProjectDetailsViewModel>
  {
    public GetProjectByIdQuery(int id)
    {
      Id = id;
    }
    public int Id { get; private set; }
  }
}