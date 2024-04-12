using Microsoft.AspNetCore.Identity;

namespace Intex.Models
{
    public class ApplicationUser : IdentityUser
    {
        // You can add additional properties here if needed
        public int CustomerId { get; set; }
    }
}