using System;
using System.Collections.Generic;
using DevFreela.Application.ViewModels;
namespace DevFreela.Application.Interfaces
{
  public interface ISkillService
  {
    List<SkillViewModel> GetAll();
  }
}