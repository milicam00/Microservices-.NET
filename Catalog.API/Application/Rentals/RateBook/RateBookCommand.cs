using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Rentals.RateBook
{
    public class RateBookCommand : CommandBase<Result>
    {
        public RateBookCommand(Guid rentalBookId, int rate, string text)
        {
            RentalBookId = rentalBookId;
            Rate = rate;
            Text = text;
        }

        public Guid RentalBookId { get; set; }
        public int Rate { get; set; }
        public string Text { get; set; }

    }
}
