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
            try
            {
                Guid userId = new Guid(userMessageDto.UserIdOrEmail);
                var user = await _userManager.FindByIdAsync(userId.ToString());
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
                    Id = Guid.NewGuid(),
                    Message = userMessageDto.Message,
                    UserId = userMessageDto.UserIdOrEmail,
                    SendUserEmail = "Anonymous"
                };
                var newUserMessage = await _userMessagesRepository.AddUserMessage(userMessage);
                return new ApiResponse<UserMessages>
                {
                    IsSuccess = true,
                    Message = "Message sent successfully",
                    StatusCode = 200,
                    ResponseObject = newUserMessage
                };
            }
            catch (Exception)
            {
                var user = await _userManager.FindByEmailAsync(userMessageDto.UserIdOrEmail);
                if (user == null)
                {
                    return new ApiResponse<UserMessages>
                    {
                        IsSuccess = false,
                        Message = "User not found",
                        StatusCode = 400
                    };
                }
                var userMessage = new UserMessages
                {
                    Id = Guid.NewGuid(),
                    Message = userMessageDto.Message,
                    UserId = user.Id,
                    SendUserEmail = "Anonymous"
                };
                var newUserMessage = await _userMessagesRepository.AddUserMessage(userMessage);
                return new ApiResponse<UserMessages>
                {
                    IsSuccess = true,
                    Message = "Message sent successfully",
                    StatusCode = 200,
                    ResponseObject = newUserMessage
                };
            }
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
                Message = "Message deleted successfully",
                StatusCode = 200,
                ResponseObject = await _userMessagesRepository.GetUserMessageById(userMessageId)
            };
        }

        public async Task<ApiResponse<UserMessages>> UpdateUserMessage(UserMessageDto userMessageDto)
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
            if (userMessageDto.Id == null)
            {
                return new ApiResponse<UserMessages>
                {
                    IsSuccess = false,
                    Message = "User message id must not be null",
                    StatusCode = 400
                };
            }
            try
            {
                Guid userId = new Guid(userMessageDto.UserIdOrEmail);
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return new ApiResponse<UserMessages>
                    {
                        IsSuccess = false,
                        Message = "User not found",
                        StatusCode = 400
                    };
                }
                var userMessage = new UserMessages
                {
                    Id = new Guid(userMessageDto.Id),
                    Message = userMessageDto.Message,
                    UserId = userMessageDto.UserIdOrEmail,
                    SendUserEmail = "Anonymous"
                };
                var updatedUser = await _userMessagesRepository.UpdateUserMessage(userMessage);
                return new ApiResponse<UserMessages>
                {
                    IsSuccess = true,
                    Message = "Message sent successfully",
                    StatusCode = 200,
                    ResponseObject = updatedUser
                };
            }
            catch (Exception)
            {
                var user = await _userManager.FindByEmailAsync(userMessageDto.UserIdOrEmail);
                if (user == null)
                {
                    return new ApiResponse<UserMessages>
                    {
                        IsSuccess = false,
                        Message = "User not found",
                        StatusCode = 400
                    };
                }
                var userMessage = new UserMessages
                {
                    Id = new Guid(userMessageDto.Id),
                    Message = userMessageDto.Message,
                    UserId = user.Id,
                    SendUserEmail = "Anonymous"
                };
                var updatedUser = await _userMessagesRepository.UpdateUserMessage(userMessage);
                return new ApiResponse<UserMessages>
                {
                    IsSuccess = true,
                    Message = "Message sent successfully",
                    StatusCode = 200,
                    ResponseObject = updatedUser
                };
            }
        }
    }
}
