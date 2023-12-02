using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.TotalRentalBooksOwner
{
    public class TotalRentalBooksOwnerQuery : QueryBase<Result>
    {
        public TotalRentalBooksOwnerQuery(string ownerUsername, DateTime startDate, DateTime endDate)
        {
            OwnerUsername = ownerUsername;
            StartDate = startDate;
            EndDate = endDate;
        }
        public string OwnerUsername { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
