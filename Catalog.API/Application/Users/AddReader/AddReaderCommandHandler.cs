using BuildingBlocks.Domain;
using Catalog.Domain.Readers;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Users.AddReader
{
    public class AddReaderCommandHandler : ICommandHandler<AddReaderCommand, Result>
    {
        private readonly IReaderRepository _readerRepository;
        public AddReaderCommandHandler(IReaderRepository readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public async Task<Result> Handle(AddReaderCommand request, CancellationToken cancellationToken)
        {
            var reader = Reader.CreateUser(
                   request.UserName,
                   request.Email,
                   request.FirstName,
                   request.LastName
                   );

            await _readerRepository.AddAsync(reader);

            return Result.Success("Successfully registration");

        }
    }

}
