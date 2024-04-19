
using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.Order
{
    public class CreateOrder
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The total amount must be higher than 0")]
        public decimal TotalAmount { get; set; }
        [Required]
        public string UserId { get; set; }

        //Shipment
        [Required]
        public string ShippingAdddress { get; set; }
        [Required]
        public string ZipCode { get; set; }

        //payment
        [Required]
        public DateTime PaymentDate { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The total amount must be higher than 0")]
        public decimal PaymentAmount { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        public string TransactionID { get; set; }
        [Required]
        public string Status { get; set; }

    }
}
