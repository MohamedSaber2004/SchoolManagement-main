using Microsoft.AspNetCore.Identity;

namespace School.Infrastructure.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public int? StudentId { get; set; }
        public string? FirebaseDeviceToken { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}
