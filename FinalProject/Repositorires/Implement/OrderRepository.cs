using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.ViewModels.Order;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repositorires.Implement
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private AppDbContext _dbContext;
        public OrderRepository(AppDbContext DbContext) : base(DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<OrderAdminDashboard> GetAdminListOrder(int page)
        {
            int pageSize = 10;
            int skipItems = (page - 1) * pageSize;
            int total = await _dbContext.Orders.CountAsync();
            var orders = await _dbContext.Orders
                .Select(o => new GetAdminOrder
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    CreatedDate = o.CreatedDate,
                    CustomerName = o.CustomerName,
                    OrderStatus = o.Status,
                    PaymentStatus = o.Payment.Status
                }).OrderByDescending(o => o.Id).AsNoTracking().ToListAsync();

            return new OrderAdminDashboard { Orders = orders, TotalOrder = total };

        }

        public async Task<IEnumerable<GetCustomerOrder>> GetCustomerOrders(string userId)
        {
           var list = await _dbContext.Orders.Where(o => o.IsDeleted == false && o.UserId == userId)
                .Select(o => new GetCustomerOrder
                {
                    Id = o.Id,
                    TotalAmount = o.TotalAmount,
                    CreatedDate = o.CreatedDate,
                    OrderStatus = o.Status,
                    EstimatedDeliveryDate = o.Shipment.EstimatedDeliveryDate,
                    TotalProduct = o.OrderDetails.Count()
                }).OrderByDescending(o => o.Id).AsNoTracking().ToListAsync();

            return list;
        }

        public async Task<GetOrderDetail> GetOrderDetail(int id)
        {
            var order = await _dbContext.Orders.Where(o => o.Id == id)
                .Select(o => new GetOrderDetail
                {
                    Id = o.Id,
                    CustomerName = o.CustomerName,
                    Email = o.Email,
                    PhoneNumber = o.PhoneNumber,
                    ShippingAdddress = o.Shipment.ShippingAdddress,
                    TotalAmount = o.TotalAmount,
                    CreatedDate = o.CreatedDate,
                    EstimatedDeliveryDate = o.Shipment.EstimatedDeliveryDate,
                    PaymentMethod = o.Payment.PaymentMethod,
                    PaymentStatus = o.Payment.Status,
                    Status = o.Status,
                    TransactionID = o.Payment.TransactionID,
                    ZipCode = o.Shipment.ZipCode,
                    ProductInOrders = o.OrderDetails.Select(od => new ProductInOrder
                    {
                        Id = od.ProductId,
                        ProductName = od.Product.Title,
                        Price = od.Product.Price,
                        Quantity = od.Quantity,
                        SubTotal = od.Quantity * od.Product.Price
                    }).ToList()
                }).FirstOrDefaultAsync();

            return order;
                
        }
    }
}
