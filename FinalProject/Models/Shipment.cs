using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Shipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ShippingAdddress { get; set; }
        public string ZipCode { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; } = DateTime.Now.AddDays(7);
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";


    }
}
