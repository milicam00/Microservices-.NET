using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAccess.API.Application.GetAllUsers;
using UserAccess.API.Controllers.Requests;
using UserAccess.API.Mediation;

namespace UserAccess.API.Controllers
{
    [Route("api/userAccess")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;

        public AdminController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] SearchUsersRequest request)
        {
            var result = await _userAccessModule.ExecuteQueryAsync(new GetAllUsersQuery(request.UserName, request.Email, request.FirstName, request.LastName, request.IsActive, request.PageNumber, request.PageSize));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
