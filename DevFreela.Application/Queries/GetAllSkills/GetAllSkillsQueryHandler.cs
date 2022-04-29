using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace DevFreela.Application.Queries.GetAllSkills
{
  public class GetAllSkillsQueryHandler : IRequestHandler<GetAllSkillsQuery, List<SkillViewModel>>
  {
    private readonly DevFreelaDbContext _dbContext;
    public GetAllSkillsQueryHandler(DevFreelaDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<List<SkillViewModel>> Handle(GetAllSkillsQuery request, CancellationToken cancellationToken)
    {
      var skills = _dbContext.Skills;
      var skillsViewModel = await skills
      .Select(s => new SkillViewModel(s.Id, s.Description))
      .ToListAsync();

      return skillsViewModel;
    }
  }
}