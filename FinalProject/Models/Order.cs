using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Printing;

namespace FinalProject.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int PaymentId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CustomerName { get; set; }
        [Column(TypeName = "Money")]
        public decimal TotalAmount { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Status { get; set; } = "Pending";
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int ShipmentId { get; set; }
        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; }
        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
