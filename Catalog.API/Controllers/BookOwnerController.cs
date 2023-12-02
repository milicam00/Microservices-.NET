using Catalog.API.Application.Books.ChangeBook;
using Catalog.API.Application.Books.CreateBook;
using Catalog.API.Application.Books.DeleteBook;
using Catalog.API.Application.Books.UploadBookImage;
using Catalog.API.Controllers.Requests;
using Catalog.API.Mediation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/books")]    
    [ApiController]
    public class BookOwnerController : ControllerBase
    {
        private readonly ICatalogModule _catalogModule;
        private readonly IConfiguration _configuration;             

        public BookOwnerController(ICatalogModule catalogModule, IConfiguration configuration)
        {
            _catalogModule = catalogModule;
            _configuration = configuration;
        }

        [Authorize(Roles = "Owner")]
        [HttpPost("add-book")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddBookAsync([FromBody] CreateBookRequest request)
        {

            var result = await _catalogModule.ExecuteCommandAsync(new CreateBookCommand(

            request.Title,
            request.Description,
            request.Author,
            request.Pages,
            request.Genres,
            request.PubblicationYear,
            request.NumberOfCopies,
            request.Library
            ));

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);

        }

        [Authorize(Roles = "Owner")]
        [HttpPut("update-book/{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditBookAsync([FromRoute] Guid bookId, [FromBody] ChangeBookRequest request)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteCommandAsync(new ChangeBookCommand(
            bookId,
            request.Title,
            request.Description,
            request.Author,
            request.Pages,
            request.PubblicationYear,
            request.UserRating,
            request.NumOfCopies,
            username));

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);


        }


        [Authorize(Roles = "Owner")]
        [HttpPut("{bookId}/remove-book")]
        public async Task<IActionResult> RemoveBookAsync([FromRoute] Guid bookId)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteCommandAsync(new DeleteBookCommand(username, bookId));
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);

        }

        [Authorize(Roles = "Owner")]
        [HttpPost("{bookId}/upload-book-image")]
        public async Task<IActionResult> UploadImageAsync([FromForm] IFormFile fileImage, [FromRoute] Guid bookId)
        {
            var username = User.Identity.Name;
            var wwwRootFolder = _configuration["AppSettings:WwwRootFolder"];
            var imageFolder = _configuration["AppSettings:BookImageFolder"];
            var result = await _catalogModule.ExecuteCommandAsync(new UploadBookImageCommand(fileImage, wwwRootFolder, imageFolder, bookId, username));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

    }
}
