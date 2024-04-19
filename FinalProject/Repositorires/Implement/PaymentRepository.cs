using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;

namespace FinalProject.Repositorires.Implement
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private AppDbContext _dbContext;
        public PaymentRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }
    }
}
