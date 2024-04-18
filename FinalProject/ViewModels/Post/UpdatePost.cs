

using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.Post
{
    public class UpdatePost
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
