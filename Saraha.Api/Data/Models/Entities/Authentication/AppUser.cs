using Microsoft.AspNetCore.Identity;

namespace Saraha.Api.Data.Models.Entities.Authentication
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExpierationDate { get; set; }
        public List<UserMessages>? Messages { get; set; }
    }
}
