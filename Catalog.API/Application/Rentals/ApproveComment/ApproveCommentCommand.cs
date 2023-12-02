using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Rentals.ApproveComment
{
    public class ApproveCommentCommand : CommandBase<Result>
    {
        public ApproveCommentCommand(Guid rentalBookId)
        {
            RentalBookId = rentalBookId;
        }
        public Guid RentalBookId { get; set; }
    }
}
