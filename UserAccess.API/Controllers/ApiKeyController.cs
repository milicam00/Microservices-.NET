using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAccess.API.Application.CreateAPIKey;
using UserAccess.API.Application.DeleteAPIKey;
using UserAccess.API.Application.EditAPIKey;
using UserAccess.API.Application.GetAllAPIKeys;
using UserAccess.API.Controllers.Requests;
using UserAccess.API.Mediation;

namespace UserAccess.API.Controllers
{
    [Route("api/userAccess/apiKey")]
    [ApiController]
    public class ApiKeyController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public ApiKeyController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        //[Authorize(Roles = "Owner")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAPIKeyAsync([FromBody] ApiKeyRequest request)
        {
           //string username = User.Identity.Name;
            var result = await _userAccessModule.ExecuteCommandAsync(new CreateAPIKeyCommand(request.Username, request.KeyName));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("update")]
        public async Task<IActionResult> EditAPIKeyAsync([FromBody] EditApiKeyRequest request)
        {
            string username = User.Identity.Name;
            var result = await _userAccessModule.ExecuteCommandAsync(new EditAPIKeyCommand(username, request.NewName));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("all-keys")]
        public async Task<IActionResult> GetAllKeysAsync([FromQuery] GetAllApiKeysRequest request)
        {
            var result = await _userAccessModule.ExecuteQueryAsync(new GetAllAPIKeysQuery(request.PageNumber, request.PageSize, request.Name));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }

        [Authorize(Roles = "Owner")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteApiKeyAsync()
        {
            string username = User.Identity.Name;
            var result = await _userAccessModule.ExecuteCommandAsync(new DeleteApiKeyCommand(username));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }

    }
}
