using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Rentals.CreateRental
{
    public class CreateRentalCommand : CommandBase<Result>
    {
        public CreateRentalCommand(
            Guid userId,
            List<Guid> bookIds

            )
        {
            UserId = userId;
            BookIds = bookIds;
        }

        public Guid UserId { get; set; }
        public List<Guid> BookIds { get; set; }

    }
}
