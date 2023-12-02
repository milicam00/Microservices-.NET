using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.GetRentalBooks
{
    public class GetRentalBooksQuery : QueryBase<List<Guid>>
    {
        public GetRentalBooksQuery(Guid rentalId)
        {
            RentalId = rentalId;
        }
        public Guid RentalId { get; set; }
    }
}
