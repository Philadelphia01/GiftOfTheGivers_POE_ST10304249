using Microsoft.AspNetCore.Identity;

namespace DisasterAlleviationFoundation.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional profile data can be added here.
        public string? FullName { get; set; }
        public string? Address { get; set; }
    }
}
