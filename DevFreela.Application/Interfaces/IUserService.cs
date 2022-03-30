using System;
using System.Collections.Generic;
using DevFreela.Application.ViewModels;
using DevFreela.Application.InputModels;
namespace DevFreela.Application.Interfaces
{
  public interface IUserService
  {
    UserViewModel GetById(int id);
    int Create(CreateUserInputModel inputModel);

  }
}