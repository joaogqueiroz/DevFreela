using System;
using System.Collections.Generic;
using System.Linq;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Application.InputModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
namespace DevFreela.Application.Services.Implementations
{
  public class UserService : IUserService
  {
    private readonly DevFreelaDbContext _dbContext;
    public UserService(DevFreelaDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public UserViewModel GetById(int id)
    {
      var user = _dbContext.Users.SingleOrDefault(u => u.Id == id);
      if (user == null)
      {
        return null;
      }
      return new UserViewModel(user.FullName, user.Email, user.BirthDate);

    }

  }
}