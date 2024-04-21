using FinalProject.Models;
using FinalProject.Repositorires.Implement;

namespace FinalProject.Repositorires.Abstraction
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUser(string userId);
    }
}
