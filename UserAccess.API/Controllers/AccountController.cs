using Microsoft.AspNetCore.Mvc;
using UserAccess.API.Application.Authentication;
using UserAccess.API.Application.ChangePassword;
using UserAccess.API.Application.ChangePersonalInfo;
using UserAccess.API.Application.GetPersonalInfo;
using UserAccess.API.Application.Logout;
using UserAccess.API.Application.ResetPassword;
using UserAccess.API.Application.ResetPasswordRequest;
using UserAccess.API.Application.TokenRefresh;
using UserAccess.API.Application.UploadProfileImage;
using UserAccess.API.Controllers.Requests;
using UserAccess.API.Mediation;

namespace UserAccess.API.Controllers
{
    [Route("api/userAccess")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;
        private readonly IConfiguration _configuration;

        public AccountController(IUserAccessModule userAccessModule, IConfiguration configuration)
        {
            _userAccessModule = userAccessModule;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var token = await _userAccessModule.ExecuteCommandAsync(new AuthenticateCommand(request.UserName, request.Password));
            if (token.IsSuccess)
            {
                return Ok(new { token });
            }

            return BadRequest(token.ErrorMessage);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        {
            
            var result = await _userAccessModule.ExecuteCommandAsync(new ChangePasswordCommand(request.Username, request.OldPassword, request.NewPassword));
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);
        }


        [HttpPost("reset-password-request")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var result = await _userAccessModule.ExecuteCommandAsync(new ResetPasswordRequestCommand(request.Username));
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);

        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] NewPasswordRequest request)
        {
            var result = await _userAccessModule.ExecuteCommandAsync(new ResetPasswordCommand(request.Code, request.Token, request.Password));
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);

        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefrehTokenAsync([FromBody] RefreshTokenRequest request)
        {
            var token = await _userAccessModule.ExecuteCommandAsync(new RefreshTokenCommand(request.RefreshToken));
            if (token.IsSuccess)
            {
                return Ok(token);
            }

            return BadRequest(token.ErrorMessage);

        }

       
        [HttpPut("change-personal-info")]
        public async Task<IActionResult> ChangeUsernameAsync([FromQuery] ChangePersonalInfoRequest request)
        {
          
            var result = await _userAccessModule.ExecuteCommandAsync(new ChangePersonalInfoCommand(request.Username, request.NewUsername, request.NewFirstName, request.NewLastName));

            return Ok(result);
        }

       
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequest logoutRequest)
        {
            var result = await _userAccessModule.ExecuteCommandAsync(new LogoutCommand(logoutRequest.RefreshToken));
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);

        }

        //[Authorize]
        [HttpPost("upload-profile-image")]
        public async Task<IActionResult> UploadProfileImageAsync([FromBody] UploadImageRequest request)
        {
            //var username = User.Identity.Name;
            var wwwRootFolder = _configuration["AppSettings:WwwRootFolder"];
            var imageFolder = _configuration["AppSettings:ProfileImageFolder"];
            var result = await _userAccessModule.ExecuteCommandAsync(new UploadProfileImageCommand(request.FileImage, wwwRootFolder, imageFolder, request.UserName));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }

     
        [HttpGet("personal-info")]
        public async Task<IActionResult> GetPersonalInfoAsync([FromQuery] PersonalInfoRequest request)
        {
            var result = await _userAccessModule.ExecuteQueryAsync(new GetPersonalInfoQuery(request.Username));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
