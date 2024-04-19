using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Services;
using FinalProject.ViewModels;
using FinalProject.ViewModels.Cart;
using FinalProject.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private const string SessionKeyName = "_Cart";
        private AppDbContext _appDbContext { get; set; }

        public OrderController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<ActionResult<Status>> PerformOrder([FromBody] CreateOrder createOrder)
        {
            Status status = new Status();
            var cart = HttpContext.Session.GetObjectFromJson<List<GetCart>>(SessionKeyName);
            if (ModelState.IsValid)
            {
                using (var transaction = _appDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Payment createPayment = new Payment
                        {
                            PaymentAmount = createOrder.PaymentAmount,
                            PaymentDate = createOrder.PaymentDate,
                            PaymentMethod = createOrder.PaymentMethod,
                            Status = createOrder.Status,
                            TransactionID = createOrder.TransactionID,
                        };
                        Shipment createShipment = new Shipment { ShippingAdddress = createOrder.ShippingAdddress, ZipCode = createOrder.ZipCode };
                        await _appDbContext.Shipments.AddAsync(createShipment);
                        await _appDbContext.Payments.AddAsync(createPayment);
                        Order order = new Order
                        {
                            TotalAmount = createOrder.TotalAmount,
                            CustomerName = createOrder.CustomerName,
                            Email = createOrder.Email,
                            PhoneNumber = createOrder.PhoneNumber,
                            Payment = createPayment,
                            Shipment = createShipment,
                            UserId = createOrder.UserId,
                        };
                        await _appDbContext.Orders.AddAsync(order);
                        await _appDbContext.SaveChangesAsync();
                        foreach (var item in cart)
                        {
                            var orderDetail = new OrderDetail
                            {
                                OrderId = order.Id,
                                ProductId = item.ProductId,
                                Quantity = item.Quantity
                            };
                            _appDbContext.OrderDetails.Add(orderDetail);
                            Product product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                            product.Quantity -= item.Quantity;
                            _appDbContext.Entry(product).State = EntityState.Modified;
                        }

                        await _appDbContext.SaveChangesAsync();
                        HttpContext.Session.SetObjectAsJson<List<GetCart>>(SessionKeyName, null);
                        transaction.Commit();
                        status.IsSuccess = true;
                        status.Message = "Adding successfully";

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            else
            {
                status.IsSuccess = false;
                status.Message = "Model is invalid";
            }

            return status;
        }
    }
}
