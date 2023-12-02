using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.GetPreviousRentalsOwner
{
    public class GetPreviousRentalsOwnerQuery : QueryBase<Result>
    {
        public GetPreviousRentalsOwnerQuery(string ownerUsername, string? libraryName, bool? isReturned, string? username, int pageNumber, int pageSize, string? orderBy)
        {
            OwnerUsername = ownerUsername;
            LibraryName = libraryName;
            IsReturned = isReturned;
            UserName = username;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
        }
        public string OwnerUsername { get; set; }
        public string? LibraryName { get; set; }
        public bool? IsReturned { get; set; }
        public string? UserName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
    }
}
