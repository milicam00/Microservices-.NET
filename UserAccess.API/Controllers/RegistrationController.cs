using Microsoft.AspNetCore.Mvc;
using UserAccess.API.Application.GetPersonalInfo;
using UserAccess.API.Application.RegisterOwner;
using UserAccess.API.Application.RegisterReader;
using UserAccess.API.Controllers.Requests;
using UserAccess.API.Mediation;

namespace UserAccess.API.Controllers
{
    [Route("api/registration")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserAccessModule _userAccessModule;
        public RegistrationController(IUserAccessModule userAccessModule)
        {
            _userAccessModule = userAccessModule;
        }


        [HttpPost("owner-registration")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> OwnerRegistrationAsync([FromBody] RegisterRequest request)
        {
            var firstTransaction = await _userAccessModule.ExecuteCommandAsync(new RegisterOwnerCommand(
                        request.Username,
                        request.Password,
                        request.Email,
                        request.FirstName,
                        request.LastName

                        ));

            if(firstTransaction.IsSuccess)
            {
                return Ok(firstTransaction);
            }
            return BadRequest(firstTransaction.ErrorMessage);
        }

        [HttpPost("reader-registration")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReaderRegistrationAsync([FromBody] RegisterRequest request)
        {
            var firstTransaction = await _userAccessModule.ExecuteCommandAsync(new RegisterReaderCommand(
                        request.Username,
                        request.Password,
                        request.Email,
                        request.FirstName,
                        request.LastName

                        ));
            if(firstTransaction.IsSuccess)
            {
                return Ok(firstTransaction);
            }
            return BadRequest(firstTransaction.ErrorMessage);
        }


    }
}
