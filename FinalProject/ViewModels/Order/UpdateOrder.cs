using Microsoft.Build.Framework;

namespace FinalProject.ViewModels.Order
{
    public class UpdateOrder
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
