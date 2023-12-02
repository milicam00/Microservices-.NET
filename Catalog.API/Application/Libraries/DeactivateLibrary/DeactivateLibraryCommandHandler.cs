using BuildingBlocks.Domain;
using Catalog.Domain.Libraries;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Libraries.DeactivateLibrary
{
    public class DeactivateLibraryCommandHandler : ICommandHandler<DeactivateLibraryCommand, Result>
    {
        public readonly ILibraryRepository _libraryRepository;
        public DeactivateLibraryCommandHandler(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<Result> Handle(DeactivateLibraryCommand request, CancellationToken cancellationToken)
        {
            Library? library = await _libraryRepository.GetByIdAsync(request.LibraryId);
            if (library == null)
            {
                return Result.Failure("This library does not exist.");
            }
            if (library.IsActive == false)
            {
                return Result.Failure("Library is already deactivated.");
            }
            library.DeactivateLibrary();
            _libraryRepository.UpdateLibrary(library);
            var libraryDto = new LibraryDto
            {
                LibraryId = library.LibraryId,
                Name = library.LibraryName,
                IsActive = library.IsActive
            };
            return Result.Success(libraryDto);

        }


    }
}
