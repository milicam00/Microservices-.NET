using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.OwnerRentals.CreateOwnerRental
{
    public class CreateOwnerRentalCommand : CommandBase<Result>
    {
        public CreateOwnerRentalCommand(string username, List<Guid> bookIds)
        {
            Username = username;
            BookIds = bookIds;
        }

        public string Username { get; set; }
        public List<Guid> BookIds { get; set; }
    }
}
