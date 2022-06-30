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
  public class UserRepository : IUserRepository
  {
    private readonly DevFreelaDbContext _dbContext;
    public UserRepository(DevFreelaDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<User> GetByIdAsync(int id)
    {
      return await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
    }
    public async Task<User> GetUserByLoginAndPasswordAsync(string email, string passwordHash)
    {
      return await _dbContext
                   .Users
                   .SingleOrDefaultAsync(u => u.Email == email && u.Password == passwordHash);
    }
    public async Task AddAsync(User user)
    {
      await _dbContext.Users.AddAsync(user);
      _dbContext.SaveChanges();
    }

  }
}