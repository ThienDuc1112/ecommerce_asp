using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.Category
{
    public class UpdateCategory
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
