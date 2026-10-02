using System.Collections.Generic;
using System.Threading.Tasks;
using DevFreela.Core.Entities;
namespace DevFreela.Core.Repositories
{
  public interface IUserRepository
  {
    Task<User> GetByIdAsync(int id);
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);

  }
}