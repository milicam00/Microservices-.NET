using BuildingBlocks.Domain;
using Catalog.Domain.OutboxMessages;
using Catalog.Domain.Owners;
using Catalog.Domain.RentalBooks;
using Catalog.Infrastructure.Configuration.Commands;
using Newtonsoft.Json;

namespace Catalog.API.Application.Rentals.ApproveComment
{
    public class ApproveCommentCommandHandler : ICommandHandler<ApproveCommentCommand, Result>
    {
        private readonly IRentalBookRepository _rentalBooksRepository;
        private readonly IOutboxMessageRepository _outboxMessageRepository;
        public ApproveCommentCommandHandler(IRentalBookRepository rentalBooksRepository, IOutboxMessageRepository outboxMessageRepository)
        {
            _rentalBooksRepository = rentalBooksRepository;
            _outboxMessageRepository = outboxMessageRepository;
        }

        public async Task<Result> Handle(ApproveCommentCommand request, CancellationToken cancellationToken)
        {
            RentalBook? rentalBook = await _rentalBooksRepository.GetByIdAsync(request.RentalBookId);
            if (rentalBook == null)
            {
                return Result.Failure("This rental does not exist.");
            }
            if (rentalBook.TextualComment == null)
            {
                return Result.Failure("This rental is not commented.");
            }

            if (rentalBook.IsCommentApproved == true)
            {
                return Result.Failure("This comment already is approved.");
            }
            rentalBook.ApproveComment();
            _rentalBooksRepository.UpdateRentalBook(rentalBook);

            ApprovedCommentDto approvedComment = new ApprovedCommentDto
            {
                Comment = rentalBook.TextualComment,
                IsCommentApproved = rentalBook.IsCommentApproved
            };


            Owner owner = await _rentalBooksRepository.GetOwner(rentalBook.BookId);

            string data = JsonConvert.SerializeObject(new
            {
                Recipient = owner.Email,
                Subject = "Approved comment",
                Body = $"Comment '{rentalBook.TextualComment}' is approved."
            });

            var outboxMessage = new OutboxMessage("Email", data);
            await _outboxMessageRepository.AddAsync(outboxMessage);



            return Result.Success(approvedComment);
        }
    }
}
