using Microsoft.AspNetCore.Identity;
using Saraha.Api.Data.DTOs;
using Saraha.Api.Data.Models.Entities;
using Saraha.Api.Data.Models.Entities.Authentication;
using Saraha.Api.Data.Models.ResponseModel;
using Saraha.Api.Repository.UserMessagesRepository;

namespace Saraha.Api.Services.UserMessagesService
{
    public class UserMessagesService : IUserMessagesService
    {
        private readonly IUserMessages _userMessagesRepository;
        private readonly UserManager<AppUser> _userManager;
        public UserMessagesService(IUserMessages _userMessagesRepository, UserManager<AppUser> _userManager)
        {
            this._userMessagesRepository = _userMessagesRepository;
            this._userManager = _userManager;
        }
        public async Task<ApiResponse<UserMessages>> AddUserMessage(UserMessageDto userMessageDto)
        {
            if (userMessageDto == null)
            {
                return new ApiResponse<UserMessages>
                {
                    IsSuccess = false,
                    Message = "Input must not be null",
                    StatusCode = 400
                };
            }
            var user = await GetUserAsync(userMessageDto.UserIdOrEmailOrUserName);
            if (user == null)
            {
                return new ApiResponse<UserMessages>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404
                };
            }
            var userMessage = new UserMessages
            {
                Message = userMessageDto.Message,
                UserId = user.Id,
                SendUserName = "Anonymous",
                ShareYourUserName = false
            };
            var sentMessage = await _userMessagesRepository.AddUserMessage(userMessage);
            return new ApiResponse<UserMessages>
            {
                IsSuccess = true, 
                Message = "Message sent successfully",
                StatusCode = 200,
                ResponseObject = sentMessage
            };
            
        }

        public async Task<ApiResponse<UserMessages>> DeleteUserMessageById(Guid userMessageId)
        {
            var userMessage = await _userMessagesRepository.GetUserMessageById(userMessageId);
            if (userMessage == null)
            {
                return new ApiResponse<UserMessages>
                {
                    IsSuccess = false,
                    Message = "User message not found",
                    StatusCode = 404,
                };
            }
            await _userMessagesRepository.DeleteUserMessageById(userMessageId);
            return new ApiResponse<UserMessages>
            {
                IsSuccess = true,
                Message = "Message deleted successfully",
                StatusCode = 200,
            };
        }

        public async Task<ApiResponse<IEnumerable<UserMessages>>> GetAllUserMessages()
        {
            var messages = await _userMessagesRepository.GetAllUserMessages();
            if (messages.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<UserMessages>>
                {
                    IsSuccess = true,
                    Message = "No messages found",
                    StatusCode = 200,
                };
            }
            return new ApiResponse<IEnumerable<UserMessages>>
            {
                IsSuccess = true,
                Message = "Messages found successfully",
                StatusCode = 200,
                ResponseObject = messages
            };
        }

        public async Task<ApiResponse<IEnumerable<UserMessages>>> GetAllUserMessagesByUserEmail(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return new ApiResponse<IEnumerable<UserMessages>>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404,
                };
            }
            var messages = await _userMessagesRepository.GetAllUserMessagesByUserEmail(userEmail);
            if (messages.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<UserMessages>>
                {
                    IsSuccess = true,
                    Message = "No messages found",
                    StatusCode = 200,
                };
            }
            return new ApiResponse<IEnumerable<UserMessages>>
            {
                IsSuccess = true,
                Message = "Messages found successfully",
                StatusCode = 200,
                ResponseObject = messages
            };
        }

        public async Task<ApiResponse<IEnumerable<UserMessages>>> GetAllUserMessagesByUserId(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<IEnumerable<UserMessages>>
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404,
                };
            }
            var messages = await _userMessagesRepository.GetAllUserMessagesByUserId(userId);
            if (messages.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<UserMessages>>
                {
                    IsSuccess = true,
                    Message = "No messages found",
                    StatusCode = 200,
                };
            }
            return new ApiResponse<IEnumerable<UserMessages>>
            {
                IsSuccess = true,
                Message = "Messages found successfully",
                StatusCode = 200,
                ResponseObject = messages
            };
        }

        public async Task<ApiResponse<UserMessages>> GetUserMessageById(Guid userMessageId)
        {
            var userMessage = await _userMessagesRepository.GetUserMessageById(userMessageId);
            if (userMessage == null)
            {
                return new ApiResponse<UserMessages>
                {
                    IsSuccess = false,
                    Message = "User message not found",
                    StatusCode = 404,
                };
            }
            return new ApiResponse<UserMessages>
            {
                IsSuccess = true,
                Message = "Message found successfully",
                StatusCode = 200,
                ResponseObject = await _userMessagesRepository.GetUserMessageById(userMessageId)
            };
        }


        private async Task<AppUser> GetUserAsync(string userIdOrUserNameOrEmail)
        {
            var userById = await _userManager.FindByIdAsync(userIdOrUserNameOrEmail);
            var userByUserName = await _userManager.FindByNameAsync(userIdOrUserNameOrEmail);
            var userByEmail = await _userManager.FindByEmailAsync(userIdOrUserNameOrEmail);
            if (userById != null)
            {
                return userById;
            }
            else if (userByEmail != null)
            {
                return userByEmail;
            }
            else if (userByUserName != null)
            {
                return userByUserName;
            }
            return null!;
        }

    }
}
