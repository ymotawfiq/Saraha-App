using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saraha.Api.Data.DTOs;
using Saraha.Api.Data.DTOs.Authentication.SignUp;
using Saraha.Api.Data.Models.Entities;
using Saraha.Api.Data.Models.Entities.Authentication;
using Saraha.Api.Data.Models.ResponseModel;
using Saraha.Api.Repository.UserMessagesRepository;
using Saraha.Api.Services.EmailService;
using Saraha.Api.Services.UserMessagesService;

namespace Saraha.Api.Controllers
{
    [ApiController]
    public class UserMessagesController : ControllerBase
    {
        private readonly IUserMessagesService _userMessagesService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserMessages _userMessagesRepository;
        private readonly IEmailService _emailService;
        public UserMessagesController(IUserMessagesService _userMessagesService,
            UserManager<AppUser> _userManager, IUserMessages _userMessagesRepository,
            IEmailService _emailService)
        {
            this._userManager = _userManager;
            this._userMessagesService = _userMessagesService;
            this._userMessagesRepository = _userMessagesRepository;
            this._emailService = _emailService;
        }


        [HttpGet("myMessages")]
        public async Task<IActionResult> GetMyMessagesAsync()
        {
            try
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        if (user.Email != null)
                        {
                            var response = await _userMessagesService.GetAllUserMessagesByUserEmail(user.Email);
                            return Ok(response);
                        }
                    }
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("usersMessages")]
        public async Task<IActionResult> GetAllUsersMessages()
        {
            try
            {
                var response = await _userMessagesService.GetAllUserMessages();
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("userMessages/{userIdOrUserNameOrEmail}")]
        public async Task<IActionResult> GetAllUsersMessages([FromRoute] string userIdOrUserNameOrEmail)
        {
            try
            {
                if(HttpContext.User.Identity!=null && HttpContext.User.Identity.Name != null)
                {
                    var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var routeUser = await GetUserAsync(userIdOrUserNameOrEmail);
                    if (currentUser != null && routeUser!=null)
                    {
                        if (currentUser.Id == routeUser.Id 
                            || await _userManager.IsInRoleAsync(currentUser, "Admin"))
                        {
                            var response = await _userMessagesService.GetAllUserMessagesByUserId(currentUser.Id);
                            return Ok(response);
                        }
                    }
                    return Unauthorized();
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessageToUser([FromBody] UserMessageDto userMessageDto)
        {
            try
            {
                if (userMessageDto.ShareYourUserName == false)
                {
                    var response = await _userMessagesService.AddUserMessage(userMessageDto);
                    userMessageDto.ShareYourUserName = false;
                    if (response.ResponseObject != null)
                    {
                        response.ResponseObject = null!;
                    }
                    return Ok(response);
                }
                if (HttpContext.User != null && HttpContext.User.Identity != null
                    && HttpContext.User.Identity.Name != null)
                {
                    var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var routeUser = await GetUserAsync(userMessageDto.UserIdOrEmailOrUserName);
                    if(currentUser!=null && routeUser != null)
                    {
                        var userMessage = new UserMessages
                        {
                            Message = userMessageDto.Message,
                            SendUserName = currentUser.UserName,
                            ShareYourUserName = true,
                            UserId = routeUser.Id
                        };
                        var response = await _userMessagesRepository.AddUserMessage(userMessage);
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                        {
                            StatusCode = 200,
                            IsSuccess = true,
                            Message = "Message sent successfully",
                        });
                    }
                    return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
                    {
                        StatusCode = 404,
                        IsSuccess = false,
                        Message = "User you want to send message to not found"
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = "Failed to send message"
                });
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }


        [Authorize(Roles = "Admin,User")]
        [HttpGet("message/{messageId}")]
        public async Task<IActionResult> GetMessage([FromRoute] Guid messageId)
        {
            try
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null)
                {
                    var message = await _userMessagesRepository.GetUserMessageById(messageId);
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null && message != null)
                    {
                        if (await _userManager.IsInRoleAsync(user, "Admin") 
                            || message.UserId == user.Id)
                        {

                            var response = await _userMessagesService.GetUserMessageById(messageId);

                            return Ok(response);
                        }
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
        }


        [Authorize(Roles = "Admin,User")]
        [HttpDelete("deleteMessage/{messageId}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] Guid messageId)
        {
            try
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null)
                {
                    var message = await _userMessagesRepository.GetUserMessageById(messageId);
                    var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    if (user != null && message != null)
                    {
                        if (await _userManager.IsInRoleAsync(user, "Admin") || message.UserId == user.Id)
                        {

                            var response = await _userMessagesService.DeleteUserMessageById(messageId);

                            return Ok(response);
                        }
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
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
