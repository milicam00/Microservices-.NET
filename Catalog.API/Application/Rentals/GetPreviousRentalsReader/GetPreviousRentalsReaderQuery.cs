using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.GetPreviousRentalsReader
{
    public class GetPreviousRentalsReaderQuery : QueryBase<Result>
    {
        public GetPreviousRentalsReaderQuery(string readerUsername, string? title, int pageNumber, int pageSize, string? orderBy)
        {
            ReaderUsername = readerUsername;
            Title = title;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
        }
        public string ReaderUsername { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Title { get; set; }
        public string? OrderBy { get; set; }
    }
}
