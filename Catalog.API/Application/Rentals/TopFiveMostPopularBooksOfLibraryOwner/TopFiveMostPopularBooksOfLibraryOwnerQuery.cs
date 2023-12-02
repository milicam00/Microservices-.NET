using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.TopFiveMostPopularBooksOfLibraryOwner
{
    public class TopFiveMostPopularBooksOfLibraryOwnerQuery : QueryBase<Result>
    {
        public TopFiveMostPopularBooksOfLibraryOwnerQuery(string ownerUsername, Guid libraryId, DateTime startDate, DateTime endDate)
        {
            OwnerUsername = ownerUsername;
            LibraryId = libraryId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public string OwnerUsername { get; set; }
        public Guid LibraryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
