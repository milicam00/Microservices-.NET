using BuildingBlocks.Domain;
using Catalog.Domain.Owners;
using Catalog.Domain.Readers;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Users.ChangeReaderPersonalInfo
{
    public class ChangeReaderPersonalInfoCommandHandler : ICommandHandler<ChangeReaderPersonalInfoCommand, Result>
    {
        private readonly IReaderRepository _readerRepository;
        private readonly IOwnerRepository _ownerRepository;
        public ChangeReaderPersonalInfoCommandHandler(IReaderRepository readerRepository, IOwnerRepository ownerRepository)
        {
            _readerRepository = readerRepository;
            _ownerRepository = ownerRepository;
        }
        public async Task<Result> Handle(ChangeReaderPersonalInfoCommand request, CancellationToken cancellationToken)
        {

            Reader reader = await _readerRepository.GetByUsername(request.OldUsername);

            if (reader == null)
            {
                return Result.Failure("Reader does  not exist.");
            }

            if (request.NewFirstName != null)
            {
                reader.ChangeFirstName(request.NewFirstName);
            }

            if (request.NewLastName != null)
            {
                reader.ChangeLastName(request.NewLastName);
            }

            if (request.NewUsername != null || request.NewUsername != reader.UserName)
            {
                Reader existingReaderWithNewUsername = await _readerRepository.GetByUsername(request.NewUsername);
                Owner existingOwnerWithNewUsername = await _ownerRepository.GetByUsername(request.NewUsername);
                if (existingReaderWithNewUsername != null || existingOwnerWithNewUsername != null)
                {
                    return Result.Failure("User with this username already exist.");
                }

                reader.ChangeUsername(request.NewUsername);

            }
            _readerRepository.Update(reader);

            return Result.Success("Successfully changed username.");


        }
    }
}
