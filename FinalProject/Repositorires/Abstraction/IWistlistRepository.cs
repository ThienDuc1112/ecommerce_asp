using FinalProject.Models;
using FinalProject.ViewModels.Whislist;

namespace FinalProject.Repositorires.Abstraction
{
    public interface IWistlistRepository : IGenericRepository<Whislist>
    {
        Task<IEnumerable<GetWistlist>> GetWhislists(string userId);
        Task<bool> IsExisted(string userId, int productId);
    }
}
