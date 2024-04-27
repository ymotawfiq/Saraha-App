using Saraha.Api.Data.Models.Entities.Authentication;

namespace Saraha.Api.Data.DTOs.Authentication.EmailConfirmation
{
    public class EmailConfirmationDto
    {
        public AppUser User { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
