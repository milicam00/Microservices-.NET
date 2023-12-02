using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Rentals.BlockComment
{
    public class BlockCommentCommand : CommandBase<Result>
    {
        public BlockCommentCommand(Guid rentalBookId)
        {
            RentalBookId = rentalBookId;
        }
        public Guid RentalBookId { get; set; }
    }
}
