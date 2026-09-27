using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DevFreela.Core.Repositories;
namespace DevFreela.Application.Queries.GetUserById
{
  public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
  {

    private readonly IUserRepository _userRepository;
    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }
    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
      var user = await _userRepository.GetByIdAsync(request.Id);
      if (user == null)
      {
        return null;
      }
      return new UserViewModel(user.FullName, user.Email, user.BirthDate);
    }
  }
}