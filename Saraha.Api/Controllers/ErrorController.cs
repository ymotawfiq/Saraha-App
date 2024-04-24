using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Saraha.Api.Data.Models.ResponseModel;

namespace Saraha.Api.Controllers
{
    [Route("error")]
    [ApiController]
    public class ErrorController : ControllerBase
    {

        [HttpGet("404")]
        public async Task<IActionResult> NotFoundAsync()
        {
            return StatusCode(StatusCodes.Status404NotFound, new ApiResponse<string>
            {
                StatusCode = 404,
                IsSuccess = false,
                Message = "Page Not found",
                ResponseObject = "Page Not found"
            });
        }

        [HttpGet("401")]
        public async Task<IActionResult> UnauthorizedAsync()
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new ApiResponse<string>
            {
                StatusCode = 401,
                IsSuccess = false,
                Message = "Unauthorized",
                ResponseObject = "Unauthorized"
            });
        }

        [HttpGet("403")]
        public async Task<IActionResult> ForbiddenAsync()
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ApiResponse<string>
            {
                StatusCode = 403,
                IsSuccess = false,
                Message = "Forbidden",
                ResponseObject = "Forbidden"
            });
        }
    }
}
