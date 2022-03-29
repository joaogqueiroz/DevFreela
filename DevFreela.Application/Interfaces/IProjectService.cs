using System;
using System.Collections.Generic;
using DevFreela.Application.ViewModels;
using DevFreela.Application.InputModels;
namespace DevFreela.Application.Interfaces
{
  public interface IProjectService
  {
    List<ProjectViewModel> GetAll(string query);
    ProjectDetailsViewModel GetById(int id);
    int Create(NewProjectInputModel inputModel);
    void Update(UpdateProjectInputModel inputModel);
    void Delete(int id);
    void CreateComment(CreateCommentInputModel inputModel);
    void Start(int id);
    void Finish(int id);
  }
}