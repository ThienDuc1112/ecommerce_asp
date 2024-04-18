using Microsoft.Build.Framework;

namespace FinalProject.ViewModels.Category
{
    public class AddCategory
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
