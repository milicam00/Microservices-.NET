using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.GetComments
{
    public class GetCommentsQuery : QueryBase<Result>
    {
        public GetCommentsQuery(string ownerUsername, string? libraryName, string? bookTitle, string? username, int pageNumber, int pageSize, string? orderBy)
        {
            OwnerUsername = ownerUsername;
            LibraryName = libraryName;
            BookTitle = bookTitle;
            UserName = username;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
        }
        public string OwnerUsername { get; set; }
        public string? LibraryName { get; set; }
        public string? BookTitle { get; set; }
        public string? UserName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
    }
}
