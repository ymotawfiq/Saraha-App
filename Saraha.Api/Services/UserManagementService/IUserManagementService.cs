using Saraha.Api.Data.DTOs.Authentication.EmailConfirmation;
using Saraha.Api.Data.DTOs.Authentication.Login;
using Saraha.Api.Data.DTOs.Authentication.ResetEmail;
using Saraha.Api.Data.DTOs.Authentication.ResetPassword;
using Saraha.Api.Data.DTOs.Authentication.SignUp;
using Saraha.Api.Data.DTOs.Authentication.User;
using Saraha.Api.Data.Models.Entities.Authentication;
using Saraha.Api.Data.Models.ResponseModel;

namespace Saraha.Api.Services.UserManagementService
{
    public interface IUserManagementService
    {
        Task<ApiResponse<CreateUserResponse>> CreateUserWithTokenAsync(RegisterUserDto registerDto);
        Task<ApiResponse<List<string>>> AssignRolesToUserAsync(List<string> roles, AppUser user);
        Task<ApiResponse<LoginOtpResponse>> LoginUserAsync(LoginUserDto loginDto);
        Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(AppUser user);
        Task<ApiResponse<LoginResponse>> LoginUserWithOTPAsync(string otp, string userNameOrEmail);
        Task<ApiResponse<LoginResponse>> RenewAccessTokenAsync(LoginResponse tokens);
        Task<ApiResponse<string>> ConfirmEmail(string userNameOrEmail, string token);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ApiResponse<ResetPasswordDto>> GenerateResetPasswordTokenAsync(string email);
        Task<ApiResponse<EmailConfirmationDto>> GenerateEmailConfirmationTokenAsync(string email);
        Task<ApiResponse<ResetEmailDto>> GenerateResetEmailTokenAsync(ResetEmailObjectDto resetEmailObjectDto);
        Task<ApiResponse<string>> EnableTwoFactorAuthenticationAsync(string email);
        Task<ApiResponse<string>> DeleteAccountAsync(string userNameOrEmail);
    }
}
