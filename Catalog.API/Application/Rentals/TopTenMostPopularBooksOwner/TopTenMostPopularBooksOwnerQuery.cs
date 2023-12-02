using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.TopTenMostPopularBooksOwner
{
    public class TopTenMostPopularBooksOwnerQuery : QueryBase<Result>
    {
        public TopTenMostPopularBooksOwnerQuery(string ownerUsername, DateTime startDate, DateTime endDate)
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
