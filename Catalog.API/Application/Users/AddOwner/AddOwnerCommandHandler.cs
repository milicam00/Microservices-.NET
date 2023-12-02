using BuildingBlocks.Domain;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Users.AddOwner
{
    public class AddOwnerCommandHandler : ICommandHandler<AddOwnerCommand, Result>
    {
        private readonly IOwnerRepository _ownerRepository;

        public AddOwnerCommandHandler(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public async Task<Result> Handle(AddOwnerCommand request, CancellationToken cancellationToken)
        {

            var owner = Owner.CreateOwner(
            request.UserName,
            request.Email,
            request.FirstName,
            request.LastName
            );
            await _ownerRepository.AddAsync(owner);

            return Result.Success("Successfully registration!");


        }
    }
}
