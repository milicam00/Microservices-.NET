using Catalog.API.Application.Libraries.DeactivateLibrary;
using Catalog.API.Application.Libraries.GetLibrariesQuery;
using Catalog.API.Mediation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/libraries")]
    [ApiController]
    public class LibraryAdminController : ControllerBase
    {
        private readonly ICatalogModule _catalogModule;

        public LibraryAdminController(ICatalogModule catalogModule)
        {
            _catalogModule = catalogModule;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetLibrariesAsync()
        {
            var libraries = await _catalogModule.ExecuteQueryAsync(new GetLibrariesQuery());
            if (libraries != null)
            {
                return Ok(libraries);
            }
            return BadRequest();

        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{libraryId}/deactivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditActivateAsync([FromRoute] Guid libraryId)
        {

            var result = await _catalogModule.ExecuteCommandAsync(new DeactivateLibraryCommand(libraryId));
            if (result.IsSuccess)
            {
                return Ok("Library is blocked.");
            }

            return BadRequest(result.ErrorMessage);


        }

    }
}
