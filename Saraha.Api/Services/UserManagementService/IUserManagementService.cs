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
        Task<ApiResponse<CreateUserResponse>> CreateUserWithTokenAsync(RegisterUserDto registerUserDto);
        Task<ApiResponse<string>> ConfirmEmail(string email, string token);
        Task<ApiResponse<List<string>>> AssignRolesToUser(List<string> roles, AppUser user);
        Task<ApiResponse<string>> ResendEmailConfirmation(string email);
        Task<ApiResponse<LoginResponse>> GetJwtTokenAsync(AppUser user);
        Task<ApiResponse<LoginOtpResponse>> GetOtpByLoginAsync(LoginUserDto loginModel);
        Task<ApiResponse<LoginResponse>> LoginUserWithJWTokenAsync(string otp, string email);
        Task<ApiResponse<LoginResponse>> RenewAccessTokenAsync(LoginResponse tokens);
        Task<ApiResponse<ResetPasswordDto>> GenerateResetPasswordTokenAsync(string email);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ApiResponse<ResetEmailDto>> GenerateResetEmailTokenAsync(ResetEmailDto resetEmail);
    }
}
