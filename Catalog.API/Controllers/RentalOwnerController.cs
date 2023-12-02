using Catalog.API.Application.OwnerRentals.CreateOwnerRental;
using Catalog.API.Application.Rentals.GetComments;
using Catalog.API.Application.Rentals.GetPreviousRentalsOwner;
using Catalog.API.Application.Rentals.GetUnreturnedBooks;
using Catalog.API.Application.Rentals.ReportComment;
using Catalog.API.Application.Rentals.TopFiveMostPopularBooksOfLibraryOwner;
using Catalog.API.Application.Rentals.TopFiveRatedBooksOfLibraryOwner;
using Catalog.API.Application.Rentals.TopFiveRatedBooksOwner;
using Catalog.API.Application.Rentals.TopTenMostPopularBooksOwner;
using Catalog.API.Application.Rentals.TotalRentalBooksOfLibraryOwner;
using Catalog.API.Application.Rentals.TotalRentalBooksOwner;
using Catalog.API.Controllers.Requests;
using Catalog.API.Mediation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/rentals")]
    [ApiController]
    public class RentalOwnerController : ControllerBase
    {
        private readonly ICatalogModule _catalogModule;

        public RentalOwnerController(ICatalogModule catalogModule)
        {
            _catalogModule = catalogModule;
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("previous-rentals-owner")]
        public async Task<IActionResult> GetPreviousRentalsOwnerAsync([FromQuery] SearchRentalOwnerRequest request)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteQueryAsync(new GetPreviousRentalsOwnerQuery(username, request.LibraryName, request.IsReturned, request.Username, request.PageNumber, request.PageSize, request.OrderBy));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("unreturned-books")]
        public async Task<IActionResult> GetUnreturnedBooksAsync([FromQuery] SearchUnreturnedBooksRequest request)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteQueryAsync(new GetUnreturnedBooksQuery(username, request.LibraryName, request.BookTitle, request.PageNumber, request.PageSize, request.OrderBy));

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);

        }

        [Authorize(Roles = "Owner")]
        [HttpPut("{rentalBookId}/report-comment")]
        public async Task<IActionResult> ReportCommentAsync([FromRoute] Guid rentalBookId)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteCommandAsync(new ReportCommentCommand(username, rentalBookId));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("comments")]
        public async Task<IActionResult> GetCommentsAsync([FromQuery] CommentSearchRequest request)
        {
            var username = User.Identity.Name;
            var result = await _catalogModule.ExecuteQueryAsync(new GetCommentsQuery(username, request.LibraryName, request.BookTitle, request.UserName, request.PageNumber, request.PageSize, request.OrderBy));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("total-rental-books-owner")]
        public async Task<IActionResult> GetTotalRentalBooksAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteQueryAsync(new TotalRentalBooksOwnerQuery(username, startDate, endDate));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);

        }

        [Authorize(Roles = "Owner")]
        [HttpGet("{libraryId}/total-rental-books-of-library-owner")]
        public async Task<IActionResult> GetTotalRentalBooksOfLibraryAsync([FromRoute] Guid libraryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteQueryAsync(new TotalRentalBooksOfLibraryOwnerQuery(username, libraryId, startDate, endDate));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);

        }

        [Authorize(Roles = "Owner")]
        [HttpGet("top-five-rated-books")]
        public async Task<IActionResult> GetTopFiveRatedBooksAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteQueryAsync(new TopFiveRatedBooksOwnerQuery(username, startDate, endDate));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("{libraryId}/top-five-rated-books-of-library-owner")]
        public async Task<IActionResult> GetTopFiveRatedBooksOfLibraryAsync([FromRoute] Guid libraryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteQueryAsync(new TopFiveRatedBooksOfLibraryOwnerQuery(username, libraryId, startDate, endDate));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }

        [Authorize(Roles = "Owner")]
        [HttpGet("top-ten-most-popular-books-owner")]
        public async Task<IActionResult> GetTopTenMostPopularBooksAsync([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteQueryAsync(new TopTenMostPopularBooksOwnerQuery(username, startDate, endDate));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);

        }

        [Authorize(Roles = "Owner")]
        [HttpGet("{libraryId}/top-ten-most-popular-books-of-library-owner")]
        public async Task<IActionResult> GetTopTenMostPopularBooksOfLibraryAsync([FromRoute] Guid libraryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var username = User.Identity.Name;

            var result = await _catalogModule.ExecuteQueryAsync(new TopFiveMostPopularBooksOfLibraryOwnerQuery(username, libraryId, startDate, endDate));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);

        }

        
        [HttpPost("rental-books")]
        public async Task<IActionResult> RentalBooks([FromBody] OwnerRentalRequest request)
        {
            string username = User.Identity.Name;
            var result = await _catalogModule.ExecuteCommandAsync(new CreateOwnerRentalCommand(username, request.BookIds));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result?.ErrorMessage);
        }
    }
}

