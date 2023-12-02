using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Books.GetBookOfLibraryOwner
{
    public class GetBooksOfLibraryOwnerQuery : QueryBase<Result>
    {
        public GetBooksOfLibraryOwnerQuery(string ownerUsername, string libraryName)
        {
            OwnerUsername = ownerUsername;
            LibraryName = libraryName;
        }
        public string OwnerUsername { get; set; }
        public string LibraryName { get; set; }
    }
}
