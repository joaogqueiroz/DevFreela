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
  public class ProjectRepository : IProjectRepository
  {
    private readonly DevFreelaDbContext _dbContext;
    public ProjectRepository(DevFreelaDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<List<Project>> GetAllAsync()
    {
      return await _dbContext.Projects.ToListAsync();
    }

    public async Task<Project> GetByIdAsync(int id)
    {
      return await _dbContext.Projects
      .Include(p => p.Client)
      .Include(p => p.Freelancer)
      .SingleOrDefaultAsync(p => p.Id == id);
    }
    public async Task AddAsync(Project project)
    {
      await _dbContext.Projects.AddAsync(project);
      await _dbContext.SaveChangesAsync();
    }
    public async Task AddCommentAsync(ProjectComment comment)
    {
      await _dbContext.ProjectComments.AddAsync(comment);
      await _dbContext.SaveChangesAsync();
    }
    public async Task SaveChangesAsync()
    {
      await _dbContext.SaveChangesAsync();
    }
  }
}