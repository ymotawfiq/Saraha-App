using System.ComponentModel.DataAnnotations;

namespace Saraha.Api.Data.DTOs
{
    public class UserMessageDto
    {
        public string? Id { get; set; }

        [Required]
        public string UserIdOrEmail { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
