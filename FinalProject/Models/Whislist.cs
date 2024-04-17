using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Whislist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
