using API_Gateway.Controllers.Account.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace API_Gateway.Controllers.Account
{
    [Route("api-gateway/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44320/api/userAccess/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return StatusCode((int)response.StatusCode, "Login error.");
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        {
            var username = User.Identity.Name;
            var req = new
            {
                Username = username,
                OldPassword = request.OldPassword,
                NewPassword = request.NewPassword
            };
            var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("https://localhost:44320/api/userAccess/change-password", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorResponse);
            }
        }


        [HttpPost("reset-password-request")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44320/api/userAccess/reset-password-request", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return StatusCode((int)response.StatusCode, "Error.");
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] NewPasswordRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("https://localhost:44320/api/userAccess/reset-password", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return StatusCode((int)response.StatusCode, "Error.");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44320/api/userAccess/refresh-token", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return StatusCode((int)response.StatusCode, "Error.");
        }

        [Authorize]
        [HttpPut("change-personal-info")]
        public async Task<IActionResult> ChangePersonalInfoAsync([FromQuery] ChangePersonalInfoRequest request)
        {
            var username = User.Identity.Name;
            var req = new
            {
                Username = username,
                NewUsername = request.NewUsername,
                NewFirstName = request.NewFirstName,
                NewLastName = request.NewLastName
            };
            var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("https://localhost:44320/api/userAccess/change-personal-info", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorResponse);
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44320/api/userAccess/logout", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return StatusCode((int)response.StatusCode, "Error.");
        }

        [Authorize]
        [HttpPost("upload-profile-image")]
        public async Task<IActionResult> UploadProfileImageAsync([FromForm] IFormFile fileImage)
        {
            var username = User.Identity.Name;
            var req = new
            {
                FileImage = fileImage,
                UserName = username
            };
            var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:44320/api/userAccess/upload-profile-image", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return StatusCode((int)response.StatusCode, "Error.");
        }

        [Authorize]
        [HttpGet("personal-info")]
        public async Task<IActionResult> GetPersonalInfoAsync()
        {
            var username = User.Identity.Name;
            var queryString = $"?Username={username}";
            var fullUrl = $"https://localhost:44320/api/userAccess/personal-info{queryString}";
            var response = await _httpClient.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return StatusCode((int)response.StatusCode, "Error.");
        }
    }

}
