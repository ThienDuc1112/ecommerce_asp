using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.Product
{
    public class UpdateProduct
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Decription { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int BrandId { get; set; }
        [Required]
        public int Quantity { get; set; }
        
    }
}
