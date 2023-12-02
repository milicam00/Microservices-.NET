using BuildingBlocks.Domain;
using Catalog.Domain.Readers;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Users.ReverseAddReader
{
    public class ReverseAddReaderCommandHandler : ICommandHandler<ReverseAddReaderCommand, Result>
    {
        private readonly IReaderRepository _readerRepository;
        public ReverseAddReaderCommandHandler(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public async Task<Result> Handle(ReverseAddReaderCommand request, CancellationToken cancellationToken)
        {

            Reader reader = await _readerRepository.GetByUsername(request.Username);
            if (reader == null)
            {
                return Result.Failure("User does not exist.");
            }
            _readerRepository.Delete(reader);
            return Result.Success("Deleted user");

        }
    }
}
