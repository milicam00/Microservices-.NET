using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.TopFiveRatedBooksOfLibrary
{
    public class TopFiveRatedBooksOfLibraryQuery : QueryBase<Result>
    {
        public TopFiveRatedBooksOfLibraryQuery(Guid libraryId, DateTime startDate, DateTime endDate)
        {
            LibraryId = libraryId;
            StartDate = startDate;
            EndDate = endDate;
        }
        public Guid LibraryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
