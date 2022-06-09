using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
namespace DevFreela.Application.Commands.CreateUser
{
  public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
  {
    private readonly DevFreelaDbContext _dbContext;
    public CreateUserCommandHandler(DevFreelaDbContext dbContext)
    {
      _dbContext = dbContext;

    }
    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
      var user = new User(request.FullName, request.Email, request.BirthDate);
      _dbContext.Users.Add(user);
      _dbContext.SaveChanges();
      return user.Id;
    }

  }
}