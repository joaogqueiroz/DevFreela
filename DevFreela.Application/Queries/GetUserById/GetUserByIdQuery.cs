using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace DevFreela.Application.Queries.GetUserById
{
  public class GetUserByIdQuery : IRequest<UserViewModel>
  {
    public GetUserByIdQuery(int id)
    {
      Id = id;
    }
    public int Id { get; private set; }
  }
}