using Saraha.Api.Data.Models.Entities.Authentication;

namespace Saraha.Api.Data.Models.Entities
{
    public class UserMessages
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? SendUserEmail { get; set; }
        public string Message { get; set; } = string.Empty;
        public AppUser? User { get; set; }
    }
}
