using Saraha.Api.Data.DTOs;
using Saraha.Api.Data.Models.Entities;
using Saraha.Api.Data.Models.ResponseModel;

namespace Saraha.Api.Services.UserMessagesService
{
    public interface IUserMessagesService
    {
        Task<ApiResponse<UserMessages>> AddUserMessage(UserMessageDto userMessageDto);
        Task<ApiResponse<UserMessages>> UpdateUserMessage(UserMessageDto userMessageDto);
        Task<ApiResponse<UserMessages>> DeleteUserMessageById(Guid userMessageId);
        Task<ApiResponse<UserMessages>> GetUserMessageById(Guid userMessageId);
        Task<ApiResponse<IEnumerable<UserMessages>>> GetAllUserMessages();
        Task<ApiResponse<IEnumerable<UserMessages>>> GetAllUserMessagesByUserId(string userId);
        Task<ApiResponse<IEnumerable<UserMessages>>> GetAllUserMessagesByUserEmail(string userEmail);
    }
}
