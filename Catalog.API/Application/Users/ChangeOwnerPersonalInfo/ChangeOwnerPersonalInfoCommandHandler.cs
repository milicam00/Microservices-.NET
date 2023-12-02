using BuildingBlocks.Domain;
using Catalog.Domain.Owners;
using Catalog.Domain.Readers;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Users.ChangeOwnerPersonalInfo
{
    public class ChangeOwnerPersonalInfoCommandHandler : ICommandHandler<ChangeOwnerPersonalInfoCommand, Result>
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IReaderRepository _readerRepository;

        public ChangeOwnerPersonalInfoCommandHandler(IOwnerRepository ownerRepository, IReaderRepository readerRepository)
        {
            _ownerRepository = ownerRepository;
            _readerRepository = readerRepository;
        }
        public async Task<Result> Handle(ChangeOwnerPersonalInfoCommand request, CancellationToken cancellationToken)
        {

            Owner owner = await _ownerRepository.GetByUsername(request.OldUsername);

            if (owner == null)
            {
                return Result.Failure("Owner does  not exist.");
            }

            if (request.NewFirstName != null)
            {
                owner.ChangeFirstName(request.NewFirstName);
            }

            if (request.NewLastName != null)
            {
                owner.ChangeLastName(request.NewLastName);
            }

            if (request.NewUsername != null || request.NewUsername != owner.UserName)
            {
                Owner existingOwnerWithNewUsername = await _ownerRepository.GetByUsername(request.NewUsername);
                Reader existingReaderWithNewUsername = await _readerRepository.GetByUsername(request.NewUsername);

                if (existingOwnerWithNewUsername != null || existingReaderWithNewUsername != null)
                {
                    return Result.Failure("Owner with this username already exist.");
                }
                owner.ChangeUsername(request.NewUsername);
            }

            _ownerRepository.Update(owner);

            return Result.Success("Successfully changed username.");


        }
    }
}
