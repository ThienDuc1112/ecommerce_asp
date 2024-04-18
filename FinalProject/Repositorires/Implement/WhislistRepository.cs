using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;

namespace FinalProject.Repositorires.Implement
{
    public class WhislistRepository : GenericRepository<Whislist>, IWhislistRepository
    {
        private readonly AppDbContext _dbContext;
        public WhislistRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }
    }
}
