using System.Collections.Generic;
using System.Threading.Tasks;
using DevFreela.Core.Entities;
namespace DevFreela.Core.Repositories
{
  public interface IUserRepository
  {
    Task<User> GetByIdAsync(int id);
    Task<User> GetUserByLoginAndPasswordAsync(string email, string passwordHash);
    Task AddAsync(User user);

  }
}