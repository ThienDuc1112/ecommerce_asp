using FinalProject.Context;
using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.Repositorires.Implement;
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
        private IOrderRepository _orderRepository;
        private AppDbContext _appDbContext { get; set; }

        public OrderController(AppDbContext appDbContext, IOrderRepository orderRepository)
        {
            _appDbContext = appDbContext;
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Status>> PerformOrder([FromBody] CreateOrder createOrder)
        {
            Status status = new Status();
            //var cart = HttpContext.Session.GetObjectFromJson<List<GetCart>>(SessionKeyName);
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
                        foreach (var item in createOrder.Carts)
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

        [HttpGet("GetOrdersByCustomer")]
        public async Task<ActionResult<IEnumerable<GetCustomerOrder>>> GetOrdersByCustomer([FromQuery(Name = "userId")] string userId)
        {
            var orders = await _orderRepository.GetCustomerOrders(userId);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpGet("GetOrdersByAdmin")]
        public async Task<ActionResult<OrderAdminDashboard>> GetOrdersByAdmin([FromQuery(Name = "page")] int page = 1)
        {
            var order = await _orderRepository.GetAdminListOrder(page);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("GetOrderDetail")]
        public async Task<ActionResult<GetOrderDetail>> GetOrderDetail([FromQuery(Name = "id")] int id)
        {
            var order = await _orderRepository.GetOrderDetail(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }


        [HttpPut("DeleteForCustomer")]
        public async Task<ActionResult> DeleteForCustomer([FromQuery(Name = "id")] int id)
        {
            var order = await _orderRepository.GetById(id);
            if (order == null)
            {
                return BadRequest("not found");
            }
            else
            {
                order.IsDeleted = true;
                await _orderRepository.Update(order);

                return NoContent();
            }
        }

        [HttpPut("Update")]
        public async Task<ActionResult<Status>> Update([FromBody] UpdateOrder updateOrder)
        {
            Status status = new Status();
            if (!ModelState.IsValid)
            {
                status.IsSuccess = false;
                status.Message = "invalid model";
                return BadRequest(status);
            }

            var order = await _orderRepository.GetById(updateOrder.Id);
            if (order == null)
            {
                status.IsSuccess = false;
                status.Message = $"can't find order with id: {updateOrder.Id}";
                return NotFound(status);
            }

            order.Status = updateOrder.Status;
            await _orderRepository.Update(order);
            status.IsSuccess = true;
            status.Message = "update successfully";
            return Ok(status);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery(Name ="id")] int id)
        {
            var order = await _orderRepository.GetById(id); 
            if(order == null)
            {
                return NotFound();
            }
            await _orderRepository.Delete(order);

            return NoContent();
        }

    }
}
