namespace Saraha.Api.Data.DTOs.Authentication.User
{
    public class TokenType
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }
}
