using Saraha.Api.Data.Models.Entities.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saraha.Api.Data.Models.Entities
{
    public class UserMessages
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? SendUserName { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool ShareYourUserName { get; set; }
        public AppUser? User { get; set; }
    }
}
