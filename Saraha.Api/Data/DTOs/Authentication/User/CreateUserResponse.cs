using Saraha.Api.Data.Models.Entities.Authentication;

namespace Saraha.Api.Data.DTOs.Authentication.User
{
    public class CreateUserResponse
    {
        public string Token { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;
    }
}
