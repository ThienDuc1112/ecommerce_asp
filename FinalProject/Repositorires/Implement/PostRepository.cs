using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;

namespace FinalProject.Repositorires.Implement
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly AppDbContext _dbContext;
        public PostRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }
    }
}
