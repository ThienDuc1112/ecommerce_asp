using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;

namespace FinalProject.Repositorires.Implement
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private AppDbContext _dbContext;
        public OrderRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }
    }
}
