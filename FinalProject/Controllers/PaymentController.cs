using FinalProject.Models;
using FinalProject.Repositorires.Abstraction;
using FinalProject.Services;
using FinalProject.ViewModels.Cart;
using FinalProject.ViewModels.Order;
using FinalProject.ViewModels.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Web;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace FinalProject.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;

        public PaymentController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
        }

   

        [HttpPost]
        public async Task<ActionResult> GetPaymentUrl([FromQuery(Name ="amount")] decimal amount)
        {
            string ipAddress = httpContextAccessor?.HttpContext?.Connection?.LocalIpAddress?.ToString();
            string url = configuration["Vnpay:PaymentUrl"];
            string returnUrl = configuration["Vnpay:ReturnUrl"];
            string tmnCode = configuration["Vnpay:TmnCode"];
            string hashSecret = configuration["Vnpay:HashSecret"];
            string total = (amount * 100).ToString();

            PayLib pay = new PayLib();
            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", tmnCode);
            pay.AddRequestData("vnp_Amount", total);
            pay.AddRequestData("vnp_BankCode", "");
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", ipAddress);
            pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", returnUrl);
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Ok(paymentUrl);
        }

        [HttpGet]
        [Route("vnpay-return")]
        public async Task<IActionResult> VnpayReturn()
        {
            if (Request.QueryString.HasValue)
            {
                string hashSecret = configuration["Vnpay:HashSecret"];
                string tmnCode = configuration["Vnpay:TmnCode"];
                var vnpayData = Request.QueryString.Value;
                var json = HttpUtility.ParseQueryString(vnpayData);

                long orderId = Convert.ToInt64(json["vnp_TxnRef"]); //mã hóa đơn
                string orderInfor = json["vnp_OrderInfo"].ToString(); //Thông tin giao dịch
                long vnpayTranId = Convert.ToInt64(json["vnp_TransactionNo"]); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = json["vnp_ResponseCode"].ToString(); 
                string vnp_SecureHash = json["vnp_SecureHash"].ToString(); 
                var pos = Request.QueryString.Value.IndexOf("&vnp_SecureHash");
                string payDate = json["vnp_PayDate"].ToString();
                string amount = json["vnp_Amount"].ToString();

                PayLib pay = new PayLib();
                bool checkSignature = pay.ValidateSignature(Request.QueryString.Value.Substring(1, pos - 1), vnp_SecureHash, hashSecret);

                if (checkSignature && tmnCode == json["vnp_TmnCode"].ToString())
                {
                    if (vnp_ResponseCode == "00")
                    {
                        PaymentStatus paymentStatus = new PaymentStatus();
                        GetPayment payment = new GetPayment
                        {
                            OrderId = orderId,
                            OrderInfor = orderInfor,
                            PaymentDate = DateTime.Now,
                            PaymentAmount = decimal.Parse(amount) / 100,
                            PaymentMethod = "VnPay",
                            Status = "success",
                            TransactionID = vnpayTranId.ToString(),
                        };
                        paymentStatus.GetPayment = payment;
                        paymentStatus.IsSuccess = true;

                        //var createOrder = HttpContext.Session.GetObjectFromJson<CreateOrder>(SessionKeyName);
                        //Payment createPayment = new Payment
                        //{
                        //    PaymentAmount = decimal.Parse(amount) / 100,
                        //    PaymentDate = DateTime.Now,
                        //    PaymentMethod = "VnPay",
                        //    Status = "success",
                        //    TransactionID = vnpayTranId.ToString(),
                        //};
                        //Shipment createShipment = new Shipment { ShippingAdddress = "abc", ZipCode = "createOrder.ZipCode" };
                        //var response1 = await _paymentRepository.Add(createPayment);
                        //var response2 = await _shipmentRepository.Add(createShipment);
                        //Order order = new Order
                        //{
                        //    TotalAmount = decimal.Parse(amount) / 100,
                        //    CustomerName = "createOrder.CustomerName",
                        //    Email = "createOrder.Email",
                        //    PhoneNumber = "createOrder.PhoneNumber",
                        //    PaymentId = createPayment.Id,
                        //    ShipmentId = createShipment.Id,
                        //    UserId = "4054c533-8ed6-45ff-b178-2eb2f7916cae",
                        //};
                        //var response3 = await _orderRepository.Add(order);                    
                        string redirectUrl = $"http://localhost:4200/paymentInfo?transactionId={vnpayTranId.ToString()}&payDate={payDate}&paymentMethod=VnPay&status=success&amount={decimal.Parse(amount) / 100}";
                        return Redirect(redirectUrl);
                    }
                    else
                    {
                        string redirectUrl = $"http://localhost:4200/failedPayment";
                        return Redirect(redirectUrl);
                    }
                }
                else
                {
                    string redirectUrl = $"http://localhost:4200/failedPayment";
                    return Redirect(redirectUrl);
                }

            }
            else
            {
                string redirectUrl = $"http://localhost:4200/failedPayment";
                return Redirect(redirectUrl);
            }
        }
       
    }
}
