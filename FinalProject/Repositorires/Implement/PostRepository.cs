using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels.Post;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositorires.Implement
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly AppDbContext _dbContext;
        public PostRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<IEnumerable<GetPost>> GetAllPosts(int productId)
        {
            return await _dbContext.Posts
                .Where(p => p.ProductId == productId)
                .Select(p => new GetPost
                {
                    Id = p.Id,
                    CreatedDate = p.CreatedDate,
                    Title = p.Title,
                    UserName = p.User.FirstName + " " + p.User.LastName
                }).ToListAsync();
        }
    }
}
