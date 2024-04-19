using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;

namespace FinalProject.Repositorires.Implement
{
    public class ShipmentRepository : GenericRepository<Shipment>, IShipmentRepository
    {
        private AppDbContext _dbContext;
        public ShipmentRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }
    }
}
