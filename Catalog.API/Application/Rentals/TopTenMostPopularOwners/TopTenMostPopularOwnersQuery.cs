using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.TopTenMostPopularOwners
{
    public class TopTenMostPopularOwnersQuery : QueryBase<Result>
    {
        public TopTenMostPopularOwnersQuery(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
