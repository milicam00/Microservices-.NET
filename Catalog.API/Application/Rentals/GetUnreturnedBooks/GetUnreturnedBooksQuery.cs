using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.GetUnreturnedBooks
{
    public class GetUnreturnedBooksQuery : QueryBase<Result>
    {
        public GetUnreturnedBooksQuery(string ownerUsername, string? libraryName, string? bookTitle, int pageNumber, int pageSize, string? orderBy)
        {
            OwnerUsername = ownerUsername;
            LibraryName = libraryName;
            BookTitle = bookTitle;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
        }
        public string OwnerUsername { get; set; }
        public string? LibraryName { get; set; }
        public string? BookTitle { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
    }
}
