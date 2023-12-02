using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Libraries.DeactivateLibrary
{
    public class DeactivateLibraryCommand : CommandBase<Result>
    {


        public DeactivateLibraryCommand(Guid libraryId)
        {
            LibraryId = libraryId;
        }

        public Guid LibraryId { get; set; }

    }
}
