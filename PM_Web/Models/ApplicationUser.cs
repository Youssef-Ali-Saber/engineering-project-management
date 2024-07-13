using Microsoft.AspNetCore.Identity;

namespace PM.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? Title { get; set; }
        public string? Organization { get; set; }
    }
}
