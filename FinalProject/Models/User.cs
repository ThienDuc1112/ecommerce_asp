using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Order> Order { get; set; }
        public List<Post> Posts { get; set; }
        public List<Whislist> Wishlists { get; set; }
        public string? Address { get; set; }
    }
}
