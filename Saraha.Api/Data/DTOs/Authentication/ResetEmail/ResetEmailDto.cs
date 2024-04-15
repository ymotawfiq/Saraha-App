namespace Saraha.Api.Data.DTOs.Authentication.ResetEmail
{
    public class ResetEmailDto
    {
        public string OldEmail { get; set; } = string.Empty;
        public string NewEmail { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
