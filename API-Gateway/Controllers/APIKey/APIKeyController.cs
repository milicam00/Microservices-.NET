using API_Gateway.Controllers.APIKey.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace API_Gateway.Controllers.APIKey
{
    [Route("api-gateway/api-key")]
    [ApiController]
    public class APIKeyController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public APIKeyController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateApiKey([FromBody] CreateApiKeyRequest request)
        {
            var username = User.Identity.Name;
            var req = new
            {
                Username = username,
                KeyName = request.KeyName
            };
            var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:44320/api/userAccess/apiKey/create", content);

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

    }
}
