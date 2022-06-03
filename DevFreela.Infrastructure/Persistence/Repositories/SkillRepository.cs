using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevFreela.Core.Entities;
using Microsoft.EntityFrameworkCore;
using DevFreela.Core.Repositories;
namespace DevFreela.Infrastructure.Persistence.Repositories
{
  public class SkillRepository : ISkillRepository
  {
    private readonly DevFreelaDbContext _dbContext;
    public SkillRepository(DevFreelaDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<List<Skill>> GetAllAsync()
    {
      return await _dbContext.Skills.ToListAsync();
    }
  }
}