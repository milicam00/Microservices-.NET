using BuildingBlocks.Domain;
using Catalog.API.Application.Books.GetAllBooks;
using Catalog.API.Application.Books.GetBookOfLibrary;
using Catalog.API.Application.Users.AddOwner;
using Catalog.API.Application.Users.AddReader;
using Catalog.API.Application.Users.BlockOwner;
using Catalog.API.Application.Users.BlockReader;
using Catalog.API.Application.Users.UnblockOwner;
using Catalog.API.Application.Users.UnblockReader;
using Catalog.API.Controllers.Requests;
using Catalog.API.Mediation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookAdminController : ControllerBase
    {
        private readonly ICatalogModule _catalogModule;
        
        public BookAdminController(ICatalogModule catalogModule)
        {
            _catalogModule = catalogModule;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> GetBooksAsync([FromQuery] PaginationFilter filter)
        {
            var books = await _catalogModule.ExecuteCommandAsync(new GetAllBooks(filter));
            if (books != null)
            {
                return Ok(books);
            }

            return BadRequest("Failed.");


        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("books-of-library/{libraryId}")]
        public async Task<IActionResult> GetBooksOfLibraryAsync([FromRoute] Guid libraryId, [FromQuery] BookSearchRequest request)
        {

            var books = await _catalogModule.ExecuteQueryAsync(new GetBooksOfLibraryQuery(libraryId, request.Title, request.Author, request.PubblicationYear, request.Genres, request.Pages, request.Rate, request.PageNumber, request.PageSize, request.OrderBy));
            if (books != null)
            {
                return Ok(books);
            }

            return BadRequest("Failed.");

        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{username}/block-owner")]
        public async Task<IActionResult> BlockOwnerAsync([FromRoute] string username)
        {
            var result = await _catalogModule.ExecuteCommandAsync(new BlockOwnerCommand(username)); 
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{username}/unblock-owner")]
        public async Task<IActionResult> UnblockOwnerAsync([FromRoute] string username)
        {
            var result = await _catalogModule.ExecuteCommandAsync(new UnblockOwnerCommand(username));
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{username}/block-reader")]
        public async Task<IActionResult> BlockReaderAsync([FromRoute] string username)
        {
            var result = await _catalogModule.ExecuteCommandAsync(new BlockReaderCommand(username));
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Administartor")]
        [HttpPut("{username}/unblock-reader")]
        public async Task<IActionResult> UnblockReaderAsync([FromRoute] string username)
        {
            var result = await _catalogModule.ExecuteCommandAsync(new UnblockReaderCommand(username));
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("registration-of-reader")]
        public async Task<IActionResult> RegisterReaderAsync([FromBody] RequestRegistration request)
        {
            var result = await _catalogModule.ExecuteCommandAsync(new AddReaderCommand(request.Username, request.Email, request.FirstName, request.LastName));
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("registration-of-owner")]
        public async Task<IActionResult> RegisterOwnerAsync([FromBody] RequestRegistration request)
        {
            var result = await _catalogModule.ExecuteCommandAsync(new AddOwnerCommand(request.Username, request.Email, request.FirstName, request.LastName));
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
