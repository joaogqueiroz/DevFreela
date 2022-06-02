using System.Collections.Generic;
using System.Threading.Tasks;
using DevFreela.Core.Entities;
namespace DevFreela.Core.Repositories
{
  public interface IProjectRepository
  {
    Task<List<Project>> GetAllAsync();
    Task<Project> GetByIdAsync(int id);
    Task AddAsync(Project project);
    Task AddCommentAsync(ProjectComment comment);
    Task SaveChangesAsync();
  }
}