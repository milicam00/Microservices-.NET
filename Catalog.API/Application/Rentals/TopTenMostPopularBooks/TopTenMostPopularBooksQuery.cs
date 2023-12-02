using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.TopTenMostPopularBooks
{
    public class TopTenMostPopularBooksQuery : QueryBase<Result>
    {
        public TopTenMostPopularBooksQuery(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
