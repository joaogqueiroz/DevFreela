using MediatR;
using System;
using System.Collections.Generic;
using DevFreela.Application.ViewModels;
namespace DevFreela.Application.Queries.GetAllSkills
{
  public class GetAllSkillsQuery : IRequest<List<SkillViewModel>>
  {
  }
}