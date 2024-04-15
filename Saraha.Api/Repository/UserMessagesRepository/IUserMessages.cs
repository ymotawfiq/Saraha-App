using Saraha.Api.Data.Models.Entities;

namespace Saraha.Api.Repository.UserMessagesRepository
{
    public interface IUserMessages
    {
        Task<UserMessages> AddUserMessage(UserMessages userMessages);
        Task<UserMessages> UpdateUserMessage(UserMessages userMessages);
        Task<UserMessages> DeleteUserMessageById(Guid userMessageId);
        Task<UserMessages> GetUserMessageById(Guid userMessageId);
        Task<IEnumerable<UserMessages>> GetAllUserMessages();
        Task<IEnumerable<UserMessages>> GetAllUserMessagesByUserId(string userId);
        Task<IEnumerable<UserMessages>> GetAllUserMessagesByUserEmail(string userEmail);
        Task SaveChangesAsync();
    }
}
