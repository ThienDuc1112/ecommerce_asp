using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.Brand
{
    public class AddBrand
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
