using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        [Column(TypeName = "Money")]
        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string? TransactionID { get; set; }
        public string Status { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
