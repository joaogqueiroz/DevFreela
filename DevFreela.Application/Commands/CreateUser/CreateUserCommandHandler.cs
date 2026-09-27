using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
namespace DevFreela.Application.Commands.CreateUser
{
  public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
  {
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;
    public CreateUserCommandHandler(IUserRepository userRepository, IAuthService authService)
    {
      _userRepository = userRepository;
      _authService = authService;

    }
    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
      var passwordHash = _authService.ComputeSha256Hash(request.Password);
      var user = new User(request.FullName, request.Email, request.BirthDate, passwordHash, request.Role);
      await _userRepository.AddAsync(user);
      return user.Id;
    }
  }
}