using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Rentals.ReportComment
{
    public class ReportCommentCommand : CommandBase<Result>
    {
        public ReportCommentCommand(string ownerUsername, Guid rentalBookId)
        {
            OwnerUsername = ownerUsername;
            RentalBookId = rentalBookId;
        }
        public string OwnerUsername { get; set; }
        public Guid RentalBookId { get; set; }
    }
}
