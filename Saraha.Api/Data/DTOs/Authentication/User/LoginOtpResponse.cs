using Saraha.Api.Data.Models.Entities.Authentication;

namespace Saraha.Api.Data.DTOs.Authentication.User
{
    public class LoginOtpResponse
    {
        public string Token { get; set; } = string.Empty;
        public bool IsTwoFactorAurhenticated { get; set; }
        public AppUser User { get; set; } = null!;
    }
}
