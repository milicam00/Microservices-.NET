using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Books.GetBooksOfOwner
{
    public class GetBooksOfLibraryOwnerQuery : QueryBase<Result>
    {
        public GetBooksOfLibraryOwnerQuery(string ownerUsername)
        {
            OwnerUsername = ownerUsername;
        }
        public string OwnerUsername { get; set; }
    }
}
