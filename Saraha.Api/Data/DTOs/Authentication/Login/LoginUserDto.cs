using System.ComponentModel.DataAnnotations;

namespace Saraha.Api.Data.DTOs.Authentication.Login
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
