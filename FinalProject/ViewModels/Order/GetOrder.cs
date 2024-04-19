using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.ViewModels.Order
{
    public class GetAdminOrder
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } 
        public string PaymentStatus { get; set; }
    }

    public class GetCustomerOrder
    {
        public int Id { get; set; }
        public int TotalProduct { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
    }

    public class OrderAdminDashboard
    {
        public List<GetAdminOrder> Orders { get; set; } = new List<GetAdminOrder>();
        public int TotalOrder { get; set; }
    }

    public class ProductInOrder
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class GetOrderDetail
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionID { get; set; }
        public string PaymentStatus { get; set; }
        public string ShippingAdddress { get; set; }
        public string ZipCode { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ProductInOrder> ProductInOrders { get; set; } = new List<ProductInOrder>();
    }
}
