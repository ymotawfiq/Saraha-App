using Microsoft.AspNetCore.Identity;

namespace Saraha.Api.Data.Models.Entities.Authentication
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpierationDate { get; set; }
        public List<UserMessages>? Messages { get; set; }
    }
}
