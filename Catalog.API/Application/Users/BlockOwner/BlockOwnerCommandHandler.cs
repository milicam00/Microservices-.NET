using BuildingBlocks.Domain;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Users.BlockOwner
{
    public class BlockOwnerCommandHandler : ICommandHandler<BlockOwnerCommand, Result>
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ILibraryRepository _libraryRepository;
        public BlockOwnerCommandHandler(IOwnerRepository ownerRepository, ILibraryRepository libraryRepository)
        {
            _ownerRepository = ownerRepository;
            _libraryRepository = libraryRepository;

        }
        public async Task<Result> Handle(BlockOwnerCommand request, CancellationToken cancellationToken)
        {
            Owner owner = await _ownerRepository.GetByUsername(request.UserName);
            if (owner == null)
            {
                return Result.Failure("User does not exist.");
            }

            if (owner.IsBlocked)
            {
                return Result.Failure("User is already blocked.");
            }
            owner.Block();

            List<Library> libraries = await _libraryRepository.GetByOwnerId(owner.OwnerId);
            if (libraries != null)
            {
                foreach (Library library in libraries)
                {
                    library.DeactivateLibrary();
                    _libraryRepository.UpdateLibrary(library);
                }
            }

            _ownerRepository.UpdateOwner(owner);

            return Result.Success("User is blocked.");


        }
    }
}
