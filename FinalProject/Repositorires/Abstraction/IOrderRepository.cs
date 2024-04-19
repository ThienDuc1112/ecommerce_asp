using FinalProject.Models;
using FinalProject.Repositorires.Implement;
using FinalProject.ViewModels.Order;

namespace FinalProject.Repositorires.Abstraction
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<OrderAdminDashboard> GetAdminListOrder(int page);
        Task<IEnumerable<GetCustomerOrder>> GetCustomerOrders(string userId);
        Task<GetOrderDetail> GetOrderDetail(int id);
    }
}
