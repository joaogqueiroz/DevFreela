using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
namespace DevFreela.Application.Commands.CreateUser
{
  public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
  {
    private readonly IUserRepository _userRepository;
    public CreateUserCommandHandler(IUserRepository userRepository)
    {
      _userRepository = userRepository;

    }
    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
      var user = new User(request.FullName, request.Email, request.BirthDate);
      await _userRepository.AddAsync(user);
      return user.Id;
    }

  }
}