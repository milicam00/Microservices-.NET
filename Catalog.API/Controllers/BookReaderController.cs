using Catalog.API.Application.Books.GetBook;
using Catalog.API.Controllers.Requests;
using Catalog.API.Mediation;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookReaderController : ControllerBase
    {
        private readonly ICatalogModule _catalogModule;
        public BookReaderController(ICatalogModule catalogModule)
        {
            _catalogModule = catalogModule;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooksAsync([FromQuery] BookSearchRequest request)
        {
            var query = new GetBookQuery(request.Title, request.Author, request.PubblicationYear, request.Genres, request.Pages, request.Rate, request.PageNumber, request.PageSize, request.OrderBy);
            var books = await _catalogModule.ExecuteQueryAsync(query);
            if (books != null)
            {
                return Ok(books);
            }

            return BadRequest("Failed");
        }
    }
}
