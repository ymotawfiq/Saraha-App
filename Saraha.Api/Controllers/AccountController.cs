using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saraha.Api.Data.DTOs.Authentication.Login;
using Saraha.Api.Data.DTOs.Authentication.ResetEmail;
using Saraha.Api.Data.DTOs.Authentication.ResetPassword;
using Saraha.Api.Data.DTOs.Authentication.SignUp;
using Saraha.Api.Data.DTOs.Authentication.User;
using Saraha.Api.Data.Models.EmailModel.MessageModel;
using Saraha.Api.Data.Models.Entities.Authentication;
using Saraha.Api.Data.Models.ResponseModel;
using Saraha.Api.Services.EmailService;
using Saraha.Api.Services.UserManagementService;
using System.Security.Claims;

namespace Saraha.Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(IUserManagementService _userManagementService,
            IEmailService _emailService, UserManager<AppUser> _userManager)
        {
            this._userManagementService = _userManagementService;
            this._emailService = _emailService;
            this._userManager = _userManager;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                var tokenResponse = await _userManagementService.CreateUserWithTokenAsync(registerUserDto);
                if (tokenResponse.IsSuccess && tokenResponse.ResponseObject != null)
                {
                    await _userManagementService
                        .AssignRolesToUser(registerUserDto.Roles, tokenResponse.ResponseObject.User);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                        new
                        {
                            token = tokenResponse.ResponseObject.Token,
                            email = registerUserDto.Email
                        }, Request.Scheme);
                    var message = new Message(new string[] { registerUserDto.Email },
                        "Confirmation Email Link", confirmationLink!);
                    var responseMessage = _emailService.SendEmail(message);
                    return StatusCode(StatusCodes.Status200OK, new ApiResponse<RegisterUserDto>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = $"Email verification link sent to ({registerUserDto.Email}) please check your inbox to verify your account"
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError,
                  new ApiResponse<RegisterUserDto>
                  {
                      Message = "Error",
                      IsSuccess = false,
                      StatusCode = 500
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

        //[Authorize(Roles ="Admin")]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            try
            {
                var response = await _userManagementService.ConfirmEmail(email, token);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Can't confirm email"
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

        [AllowAnonymous]
        [HttpGet("resendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmatiomAsync(string email)
        {
            try
            {
                var emailConfirmation = await _userManagementService.ResendEmailConfirmation(email);
                if (emailConfirmation.IsSuccess)
                {
                    if (emailConfirmation.ResponseObject != null)
                    {
                        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                        new
                        {
                            token = emailConfirmation.ResponseObject,
                            email = email
                        }, Request.Scheme);
                        var message = new Message(new string[] { email },
                            "Confirmation Email Link", confirmationLink!);
                        var responseMessage = _emailService.SendEmail(message);
                        return Ok(emailConfirmation);
                    }
                }

                return Ok(emailConfirmation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    StatusCode = 500,
                    IsSuccess = true,
                    Message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                var loginOtpResponse = await _userManagementService.GetOtpByLoginAsync(loginUserDto);
                if (loginOtpResponse.ResponseObject != null)
                {
                    var user = loginOtpResponse.ResponseObject.User;
                    if (user.TwoFactorEnabled)
                    {
                        var token = loginOtpResponse.ResponseObject.Token;
                        var message = new Message(new string[] { user.Email! }, "OTP Confrimation", token);
                        _emailService.SendEmail(message);

                        return StatusCode(StatusCodes.Status200OK,
                         new ApiResponse<LoginUserDto>
                         {
                             IsSuccess = loginOtpResponse.IsSuccess,
                             StatusCode = 200,
                             Message = $"We have sent an OTP to your Email {user.Email}"
                         });
                    }
                    if (user != null && await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
                    {
                        var serviceResponse = await _userManagementService.GetJwtTokenAsync(user);
                        return Ok(serviceResponse);
                    }
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<LoginUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("login-2FA")]
        public async Task<IActionResult> LoginWithTwoFactorAuthenticationAsync(string otp, string email)
        {
            try
            {
                var jwt = await _userManagementService.LoginUserWithJWTokenAsync(otp, email);
                if (jwt.IsSuccess)
                {
                    return Ok(jwt);
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
                {
                    StatusCode = 404,
                    IsSuccess = false,
                    Message = "Invalid OTP"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<LoginUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles ="Admin,User")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] LoginResponse loginResponse)
        {
            try
            {
                var jwt = await _userManagementService.RenewAccessTokenAsync(loginResponse);
                if (jwt.IsSuccess)
                {
                    return Ok(jwt);
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Can't generate refresh token"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<LoginUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles ="Admin,User")]
        [HttpGet("logout")]
        public async Task<string> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return "Success";
        }

        [HttpGet("current-user")]
        public ClaimsPrincipal GetCurrentUserAsync()
        {
            return HttpContext.User;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("generateResetPasswordObject")]
        public async Task<IActionResult> GenerateResetPasswordObject(string email, string token)
        {
            try
            {
                var resetPasswordObject = new ResetPasswordDto
                {
                    Email = email,
                    Token = token
                };
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<ResetPasswordDto>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Message = "Object generatted successfully",
                    ResponseObject = resetPasswordObject
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

        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            try
            {
                var response = await _userManagementService.GenerateResetPasswordTokenAsync(email);
                if (response.IsSuccess)
                {
                    if (response.ResponseObject != null)
                    {
                        var forgetPasswordLink = Url.Action(nameof(GenerateResetPasswordObject), "Account",
                        new { email = response.ResponseObject.Email, token = response.ResponseObject.Token }
                        , Request.Scheme);


                        var message = new Message(new string[] { response.ResponseObject.Email! }
                        , "Forget password link", forgetPasswordLink);
                        _emailService.SendEmail(message);
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse<LoginUserDto>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Message = "Forget password link sent successfully to your email"
                        });
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<LoginUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Can't send forget password link to your email"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<LoginUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }


        [AllowAnonymous]
        [HttpGet("resendForgetPasswordLink")]
        public async Task<IActionResult> ResendForgetPasswordLinkAsync(string email)
        {
            try
            {
                var response = await _userManagementService.GenerateResetPasswordTokenAsync(email);
                if (response.IsSuccess)
                {
                    if (response.ResponseObject != null)
                    {
                        var forgetPasswordLink = Url.Action(nameof(GenerateResetPasswordObject), "Account",
                        new { email = response.ResponseObject.Email, token = response.ResponseObject.Token }
                        , Request.Scheme);


                        var message = new Message(new string[] { response.ResponseObject.Email! }
                        , "Forget password link", forgetPasswordLink);
                        _emailService.SendEmail(message);
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse<LoginUserDto>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Message = "Forget password link sent successfully to your email"
                        });
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<LoginUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Can't send forget password link to your email"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<LoginUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var response = await _userManagementService.ResetPasswordAsync(resetPasswordDto);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<LoginUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Failed to reset password"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<LoginUserDto>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("sendEmailToResetEmail")]
        public async Task<IActionResult> SendEmailToResetEmailAsync(string oldEmail, string newEmail)
        {
            try
            {
                var response = await _userManagementService.GenerateResetEmailTokenAsync(
                new ResetEmailDto
                {
                    OldEmail = oldEmail,
                    NewEmail = newEmail
                });
                if (response.IsSuccess)
                {
                    if (response.ResponseObject != null)
                    {
                        var emailResetLink = Url.Action(nameof(GenerateEmailResetObject), "Account",
                        new { OldEmail = oldEmail, NewEmail = newEmail, token = response.ResponseObject.Token }
                        , Request.Scheme);


                        var message = new Message(new string[] { response.ResponseObject.OldEmail! }
                        , "Email reser link", emailResetLink);
                        _emailService.SendEmail(message);
                        return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                        {
                            StatusCode = 200,
                            IsSuccess = true,
                            Message = "Email rest link sent to your email"
                        });
                    }
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Can't send reset email link"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("generateemailresetobject")]
        public async Task<IActionResult> GenerateEmailResetObject(string OldEmail, string NewEmail, string token)
        {
            var resetEmailObject = new ResetEmailDto
            {
                NewEmail = NewEmail,
                Token = token,
                OldEmail = OldEmail
            };
            return StatusCode(StatusCodes.Status200OK, new ApiResponse<ResetEmailDto>
            {
                StatusCode = 200,
                IsSuccess = true,
                Message = "Reset email object created",
                ResponseObject = resetEmailObject
            });
        }

        [AllowAnonymous]
        [HttpPost("reset-email")]
        public async Task<IActionResult> ResetEmailAsync([FromBody] ResetEmailDto resetEmail)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(resetEmail.OldEmail);
                if (user != null)
                {
                    await _userManager.ChangeEmailAsync(user, resetEmail.NewEmail, resetEmail.Token);
                    user.UserName = resetEmail.NewEmail;
                    await _userManager.UpdateAsync(user);
                    return StatusCode(StatusCodes.Status200OK, new ApiResponse<string>
                    {
                        StatusCode = 200,
                        IsSuccess = true,
                        Message = "Email changed successfully"
                    });
                }
                return StatusCode(StatusCodes.Status400BadRequest, new ApiResponse<string>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    Message = "Unable to reset email"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                });
            }
        }


    }
}
