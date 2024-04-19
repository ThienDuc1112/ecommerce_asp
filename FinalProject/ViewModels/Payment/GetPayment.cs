using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.ViewModels.Payment
{
    public class GetPayment
    {
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionID { get; set; }
        public string Status { get; set; }
        public long OrderId { get; set; }
        public string OrderInfor { get; set; }
    }
}
