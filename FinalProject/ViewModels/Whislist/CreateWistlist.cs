using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels.Whislist
{
    public class CreateWistlist
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The ProductId must be higher than 0")]
        public int ProductId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
