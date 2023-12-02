using API_Gateway.Controllers.Registration.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace API_Gateway.Controllers.Registration
{
    [Route("api-gateway/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public RegistrationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("owner")]
        public async Task<IActionResult> OwnerRegistrationAsync([FromBody] RegisterRequest request)
        {
            var contentUser = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var req = new
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var contentCatalog = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            var responseUserAccess = await _httpClient.PostAsync("https://localhost:44320/api/registration/owner-registration", contentUser);
            var responseCatalog = await _httpClient.PostAsync("https://localhost:44319/api/books/registration-of-owner", contentCatalog);
            if (responseUserAccess.IsSuccessStatusCode && responseCatalog.IsSuccessStatusCode)
            {
                var result = await responseUserAccess.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return StatusCode((int)responseUserAccess.StatusCode, "Registration error.");

        }

        [HttpPost("reader")]
        public async Task<IActionResult> ReaderRegistrationAsync([FromBody] RegisterRequest request)
        {
            var contentUser = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var req = new
            {
                Username = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var contentCatalog = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
            var responseUserAccess = await _httpClient.PostAsync("https://localhost:44320/api/registration/reader-registration", contentUser);
            var responseCatalog = await _httpClient.PostAsync("https://localhost:44319/api/books/registration-of-reader", contentCatalog);
            if (responseUserAccess.IsSuccessStatusCode && responseCatalog.IsSuccessStatusCode)
            {
                var result = await responseUserAccess.Content.ReadAsStringAsync();
                return Ok(result);
            }
            return StatusCode((int)responseUserAccess.StatusCode, "Registration error.");

        }


    }
}
