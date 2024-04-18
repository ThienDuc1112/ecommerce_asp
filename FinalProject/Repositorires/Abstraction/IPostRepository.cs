using FinalProject.Models;
using FinalProject.Repositorires.Implement;
using FinalProject.ViewModels.Post;

namespace FinalProject.Repositorires.Abstraction
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<IEnumerable<GetPost>> GetAllPosts(int productId);
    }
}
