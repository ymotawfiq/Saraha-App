using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Saraha.Api.Data.DTOs.Authentication.Login;
using Saraha.Api.Data.DTOs.Authentication.ResetEmail;
using Saraha.Api.Data.DTOs.Authentication.ResetPassword;
using Saraha.Api.Data.DTOs.Authentication.SignUp;
using Saraha.Api.Data.DTOs.Authentication.User;
using Saraha.Api.Data.Models.Entities.Authentication;
using Saraha.Api.Data.Models.ResponseModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Saraha.Api.Services.UserManagementService
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        public UserManagementService(UserManager<AppUser> _userManager,
            RoleManager<IdentityRole> _roleManager, IConfiguration _configuration,
            SignInManager<AppUser> _signInManager)
        {
            this._userManager = _userManager;
            this._roleManager = _roleManager;
            this._configuration = _configuration;
            this._signInManager = _signInManager;
        }

        public async Task<ApiResponse<List<string>>> AssignRolesToUser(List<string> roles, AppUser user)
        {
            List<string> assignRoles = new();
            foreach(var role in roles)
            {
                if(await _roleManager.RoleExistsAsync(role))
                {
                    if(!await _userManager.IsInRoleAsync(user, role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                        assignRoles.Add(role);
                    }
                }
            }
            return new ApiResponse<List<string>>
            {
                IsSuccess = true,
                Message = "Roles assigned successfully to user",
                StatusCode = 200,
                ResponseObject = assignRoles
            };
        }

        public async Task<ApiResponse<string>> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var confirmEmail = await _userManager.ConfirmEmailAsync(user, token);
                if (confirmEmail.Succeeded)
                {
                    return new ApiResponse<string>
                    {
                        IsSuccess = true,
                        Message = "Email confirmed successfully",
                        StatusCode = 200
                    };
                }
            }
            return new ApiResponse<string>
            {
                IsSuccess = false,
                Message = "Can't confirm email",
                StatusCode = 400
            };
        }

        public async Task<ApiResponse<CreateUserResponse>> CreateUserWithTokenAsync(RegisterUserDto registerUserDto)
        {
            var existUser = await _userManager.FindByEmailAsync(registerUserDto.Email);
            if (existUser != null)
            {
                return new ApiResponse<CreateUserResponse>
                {
                    IsSuccess = false,
                    Message = "Choose another email",
                    StatusCode = 403
                };
            }
            AppUser user = new AppUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                TwoFactorEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, registerUserDto.Password);
            if (result.Succeeded)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return new ApiResponse<CreateUserResponse>
                {
                    IsSuccess = true,
                    Message = "User created successfully",
                    StatusCode = 201,
                    ResponseObject = new CreateUserResponse
                    {
                        Token = token,
                        User = user
                    }
                };
            }
            return new ApiResponse<CreateUserResponse>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = "Failed to create user",
                ResponseObject = new()
            };
        }

        public async Task<ApiResponse<ResetEmailDto>> GenerateResetEmailTokenAsync(ResetEmailDto resetEmail)
        {
            var oldUser = await _userManager.FindByEmailAsync(resetEmail.OldEmail);
            var newUser = await _userManager.FindByEmailAsync(resetEmail.NewEmail);
            if (oldUser == null || newUser != null)
            {
                return new ApiResponse<ResetEmailDto>
                {
                    IsSuccess = false,
                    Message = "can't generate reset email token",
                    StatusCode = 400
                };
            }
            var token = await _userManager.GenerateChangeEmailTokenAsync(oldUser, resetEmail.NewEmail);
            resetEmail.Token = token;
            return new ApiResponse<ResetEmailDto>
            {
                IsSuccess = true,
                Message = "Reset email link sent successfully to your email pleace check inbox and reset your email",
                StatusCode = 200,
                ResponseObject = resetEmail
            };
        }

        public async Task<ApiResponse<ResetPasswordDto>> GenerateResetPasswordTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                return new ApiResponse<ResetPasswordDto>
                {
                    IsSuccess = true,
                    Message = "Reset password token generated successfully",
                    StatusCode = 200,
                    ResponseObject = new ResetPasswordDto
                    {
                        Token = token,
                        Email = email
                    }
                };
            }
            return new ApiResponse<ResetPasswordDto>
            {
                IsSuccess = false,
                Message = "Can't generate reset password token",
                StatusCode = 400
            };
        }

        public async Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(AppUser user)
        {
            if (user.UserName == null)
            {
                throw new NullReferenceException("Username must not be null");
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var jwtToken = GetToken(authClaims);
            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidity"], out int refreshTokenValidity);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpierationDate = DateTime.UtcNow.AddDays(refreshTokenValidity);
            await _userManager.UpdateAsync(user);
            return new ApiResponse<LoginResponse>
            {
                IsSuccess = true,
                Message = "Token created",
                StatusCode = 200,
                ResponseObject = new LoginResponse
                {
                    AccessToken = new TokenType()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        ExpiryDate = jwtToken.ValidTo
                    },
                    RefreshToken = new TokenType()
                    {
                        Token = user.RefreshToken,
                        ExpiryDate = (DateTime)user.RefreshTokenExpierationDate
                    }
                }
            };
        }

        public async Task<ApiResponse<LoginOtpResponse>> GetOtpByLoginAsync(LoginUserDto loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true);
                if (user.TwoFactorEnabled)
                {
                    if (!user.EmailConfirmed)
                    {
                        return new ApiResponse<LoginOtpResponse>
                        {
                            IsSuccess = false,
                            StatusCode = 400,
                            Message = "Please confirm your email"
                        };
                    }
                    var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                    return new ApiResponse<LoginOtpResponse>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Message = "OTP sent to your email",
                        ResponseObject = new LoginOtpResponse
                        {
                            IsTwoFactorAurhenticated = user.TwoFactorEnabled,
                            User = user,
                            Token = token
                        }
                    };
                }
                else
                {
                    return new ApiResponse<LoginOtpResponse>
                    {
                        IsSuccess = true,
                        Message = "Two factor authentication not enabled",
                        StatusCode = 200,
                        ResponseObject = new LoginOtpResponse
                        {
                            IsTwoFactorAurhenticated = user.TwoFactorEnabled,
                            Token = string.Empty,
                            User = user
                        }
                    };
                }
            }

            return new ApiResponse<LoginOtpResponse>
            {
                IsSuccess = false,
                StatusCode = 404,
                Message = $"User doesnot exist."
            };

        }

        public async Task<ApiResponse<LoginResponse>> LoginUserWithJWTokenAsync(string otp, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var signIn = await _signInManager.TwoFactorSignInAsync("Email", otp, true, false);
            if (signIn.Succeeded)
            {
                if (user != null)
                {
                    return await GetJwtTokenAsync(user);
                }
            }
            return new ApiResponse<LoginResponse>
            {
                ResponseObject = new LoginResponse()
                {

                },
                IsSuccess = false,
                StatusCode = 400,
                Message = $"Invalid Otp"
            };
        }

        public async Task<ApiResponse<LoginResponse>> RenewAccessTokenAsync(LoginResponse tokens)
        {
            var accessToken = tokens.AccessToken;
            var refreshToken = tokens.RefreshToken;
            if (accessToken == null || refreshToken == null || refreshToken.Token == null)
            {
                throw new NullReferenceException();
            }
            var principal = GetClaimsPrincipal(accessToken.Token);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if(refreshToken.Token != user.RefreshToken || refreshToken.ExpiryDate <= DateTime.Now)
            {
                return new ApiResponse<LoginResponse>
                {

                    IsSuccess = false,
                    StatusCode = 400,
                    Message = $"Token invalid or expired"
                };
            }
            var response = await GetJwtTokenAsync(user);
            return response;
        }

        public async Task<ApiResponse<string>> ResendEmailConfirmation(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    return new ApiResponse<string>
                    {
                        IsSuccess = false,
                        Message = "Can't send email confirmation",
                        StatusCode = 400
                    };
                }
                else
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    return new ApiResponse<string>
                    {
                        IsSuccess = true,
                        Message = "Email confirmation sent successfully pleace check your inbox to verify",
                        StatusCode = 200,
                        ResponseObject = token
                    };
                }
            }
            return new ApiResponse<string>
            {
                IsSuccess = false,
                Message = "Can't send email confirmation",
                StatusCode = 400
            };
        }

        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user != null)
            {
                var resetPassword = await _userManager
                    .ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
                if (resetPassword.Succeeded)
                {
                    return new ApiResponse<string>
                    {
                        IsSuccess = true,
                        Message = "Password reset successfully",
                        StatusCode = 200
                    };
                }
                return new ApiResponse<string>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Failed to reset password",
                    ResponseObject = resetPassword.Errors.ToString()
                };
            }
            return new ApiResponse<string>
            {
                StatusCode = 400,
                IsSuccess = false,
                Message = "Can't send link to email please try again"
            };
        }

        ////
        ///
#region PrivateMethods
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            var expirationTimeUtc = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);
            var localTimeZone = TimeZoneInfo.Local;
            var expirationTimeInLocalTimeZone = TimeZoneInfo.ConvertTimeFromUtc(expirationTimeUtc, localTimeZone);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: expirationTimeInLocalTimeZone,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[64];
            var range = RandomNumberGenerator.Create();
            range.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetClaimsPrincipal(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            return principal;

        }

        #endregion


    }
}
