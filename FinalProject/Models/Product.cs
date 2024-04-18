using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "Money")]
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Decription { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
        public List<Post> Posts { get; set; }

    }
}
