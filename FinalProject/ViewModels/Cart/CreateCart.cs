using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.Cart
{
    public class CreateCart
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The ProductId must be higher than 0")]
        public int ProductId { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The quantity must be higher than 0")]
        public int Quantity { get; set; }
    }
}
