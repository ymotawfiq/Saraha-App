using Saraha.Api.Data.Models.EmailModel.MessageModel;

namespace Saraha.Api.Services.EmailService
{
    public interface IEmailService
    {
        string SendEmail(Message message);
    }
}
