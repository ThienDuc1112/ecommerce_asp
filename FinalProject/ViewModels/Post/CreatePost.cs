

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FinalProject.ViewModels.Post
{
    public class CreatePost
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The ProductId must be higher than 0")]
        public int ProductId { get; set; }
    }
}
