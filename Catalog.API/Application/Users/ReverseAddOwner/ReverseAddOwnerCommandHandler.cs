using BuildingBlocks.Domain;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Users.ReverseAddOwner
{
    public class ReverseAddOwnerCommandHandler : ICommandHandler<ReverseAddOwnerCommand, Result>
    {
        private readonly IOwnerRepository _ownerRepository;

        public ReverseAddOwnerCommandHandler(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<Result> Handle(ReverseAddOwnerCommand request, CancellationToken cancellationToken)
        {

            Owner owner = await _ownerRepository.GetByUsername(request.Username);
            if (owner == null)
            {
                return Result.Failure("User does not exist.");
            }
            _ownerRepository.Delete(owner);
            return Result.Success("Deleted user");

        }
    }
}
