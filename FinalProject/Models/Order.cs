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
        public DateTime CreatedDate { get; set; }
        public int Payment { get; set; }
        public string PhoneNumber { get; set; }
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
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
