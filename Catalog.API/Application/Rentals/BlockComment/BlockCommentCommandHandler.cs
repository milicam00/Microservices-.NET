using BuildingBlocks.Domain;
using Catalog.Domain.OutboxMessages;
using Catalog.Domain.Owners;
using Catalog.Domain.Readers;
using Catalog.Domain.RentalBooks;
using Catalog.Infrastructure.Configuration.Commands;
using Newtonsoft.Json;

namespace Catalog.API.Application.Rentals.BlockComment
{
    public class BlockCommentCommandHandler : ICommandHandler<BlockCommentCommand, Result>
    {
        private readonly IRentalBookRepository _rentalBooksRepository;
        private readonly IOutboxMessageRepository _outboxMessageRepository;
        public BlockCommentCommandHandler(IRentalBookRepository rentalBooksRepository, IOutboxMessageRepository outboxMessageRepository)
        {
            _rentalBooksRepository = rentalBooksRepository;
            _outboxMessageRepository = outboxMessageRepository;
        }

        public async Task<Result> Handle(BlockCommentCommand request, CancellationToken cancellationToken)
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

            if (rentalBook.IsCommentApproved == false)
            {
                return Result.Failure("This comment already is blocked.");
            }
            rentalBook.BlockComment();
            _rentalBooksRepository.UpdateRentalBook(rentalBook);

            BlockedCommentDto blockedComment = new BlockedCommentDto
            {
                Comment = rentalBook.TextualComment,
                IsCommentApproved = rentalBook.IsCommentApproved
            };

            Reader reader = await _rentalBooksRepository.GetReader(rentalBook.RentalId);
            Owner owner = await _rentalBooksRepository.GetOwner(rentalBook.BookId);

            string dataReader = JsonConvert.SerializeObject(new
            {
                Recipient = reader.Email,
                Subject = "Blocked comment",
                Body = $"Your comment: '{rentalBook.TextualComment}' is blocked."
            });
            string dataOwner = JsonConvert.SerializeObject(new
            {
                Recipient = owner.Email,
                Subject = "Blocked comment",
                Body = $"Your comment: '{rentalBook.TextualComment}' is blocked."
            });
            var outboxMessageReader = new OutboxMessage("Email", dataReader);
            var outboxMessageOwner = new OutboxMessage("Email", dataOwner);
            List<OutboxMessage> outboxMessages = new List<OutboxMessage>();
            outboxMessages.Add(outboxMessageReader);
            outboxMessages.Add(outboxMessageOwner);
            await _outboxMessageRepository.AddAsync(outboxMessages);



            return Result.Success(blockedComment);
        }
    }
}
