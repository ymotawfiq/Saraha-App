using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saraha.Api.Data.DTOs;
using Saraha.Api.Data.Models.Entities;
using Saraha.Api.Data.Models.Entities.Authentication;
using Saraha.Api.Data.Models.ResponseModel;
using Saraha.Api.Repository.UserMessagesRepository;
using Saraha.Api.Services.UserMessagesService;

namespace Saraha.Api.Controllers
{
    [ApiController]
    public class UserMessagesController : ControllerBase
    {
        private readonly IUserMessagesService _userMessagesService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserMessages _userMessagesRepository;
        public UserMessagesController(IUserMessagesService _userMessagesService,
            UserManager<AppUser> _userManager, IUserMessages _userMessagesRepository)
        {
            this._userManager = _userManager;
            this._userMessagesService = _userMessagesService;
            this._userMessagesRepository = _userMessagesRepository;
        }

        [HttpGet("admins")]
        public async Task<IActionResult> Admins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return Ok(admins);
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
        [HttpGet("usersMessagesByuser/{userId}")]
        public async Task<IActionResult> GetAllUsersMessagesByUserId([FromRoute] string userId)
        {
            try
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null)
                {
                    var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var admins = await _userManager.GetUsersInRoleAsync("Admin");
                        if (user.Id == userId || admins.Contains(user))
                        {
                            var response = await _userMessagesService.GetAllUserMessagesByUserId(userId);
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
        [HttpGet("usersMessagesByuser")]
        public async Task<IActionResult> GetAllUsersMessagesByUserRmail(string userEmail)
        {
            try
            {
                if (HttpContext.User.Identity != null && HttpContext.User.Identity.Name!=null)
                {
                    var user = await _userManager.FindByEmailAsync(HttpContext.User.Identity.Name);
                    if (user != null)
                    {
                        var admins = await _userManager.GetUsersInRoleAsync("Admin");
                        if (user.Email == userEmail || admins.Contains(user))
                        {
                            var response = await _userMessagesService.GetAllUserMessagesByUserEmail(userEmail);
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

        [AllowAnonymous]
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessageToUser([FromBody] UserMessageDto userMessageDto)
        {
            try
            {
                var response = await _userMessagesService.AddUserMessage(userMessageDto);
                return Ok(response);
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

        [Authorize(Roles ="Admin,User")]
        [HttpDelete("deleteMessage/{messageId}")]
        public async Task<IActionResult> DeleteMessage([FromRoute] Guid messageId)
        {
            try
            {
                var message = await _userMessagesRepository.GetUserMessageById(messageId);
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user != null && message != null)
                {
                    var admins = await _userManager.GetUsersInRoleAsync("Admin");
                    if(admins.Contains(user) || message.UserId == user.Id)
                    {

                        var response = await _userMessagesService.DeleteUserMessageById(messageId);

                        return Ok(response);
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


    }
}
