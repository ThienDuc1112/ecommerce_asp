using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositorires.Implement
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private AppDbContext _dbContext;
        public UserRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<User> GetUser(string userId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
