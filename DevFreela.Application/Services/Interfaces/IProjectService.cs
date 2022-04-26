using System;
using System.Collections.Generic;
using DevFreela.Application.ViewModels;
using DevFreela.Application.InputModels;
namespace DevFreela.Application.Services.Interfaces
{
  public interface IProjectService
  {
    List<ProjectViewModel> GetAll();
    ProjectDetailsViewModel GetById(int id);
  }
}