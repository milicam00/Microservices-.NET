using BuildingBlocks.Domain;
using Catalog.Domain.Readers;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Users.UnblockReader
{
    public class UnblockReaderCommandHandler : ICommandHandler<UnblockReaderCommand, Result>
    {
        private readonly IReaderRepository _readerRepository;

        public UnblockReaderCommandHandler(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public async Task<Result> Handle(UnblockReaderCommand request, CancellationToken cancellationToken)
        {

            Reader reader = await _readerRepository.GetByUsername(request.UserName);
            if (reader == null)
            {
                return Result.Failure("User does not exist.");
            }
            if (!reader.IsBlocked)
            {
                return Result.Failure("User is already unblocked.");
            }

            reader.Unblock();
            _readerRepository.Update(reader);
            return Result.Success("User is unblocked.");

        }
    }
}
