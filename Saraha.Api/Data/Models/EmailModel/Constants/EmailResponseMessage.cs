namespace Saraha.Api.Data.Models.EmailModel.Constants
{
    public static class EmailResponseMessage
    {
        public static string GetEmailSuccessMeggage(string email)
        {
            return $"Email sent successfully to {email}";
        }
    }
}
