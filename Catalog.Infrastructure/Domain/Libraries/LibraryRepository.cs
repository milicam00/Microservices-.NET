using Catalog.Domain.Libraries;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Domain.Libraries
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly CatalogContext _catalogContext;

        public LibraryRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task AddAsync(Library library)
        {
            await _catalogContext.Libraries.AddAsync(library);
        }

        public async Task<Library?> GetByIdAsync(Guid libraryId)
        {
            return await _catalogContext.Libraries.FirstOrDefaultAsync(x => x.LibraryId == libraryId);
        }

        public async Task<List<Library>> GetByIdsAsync(List<Guid> libraryIds)
        {
            return await _catalogContext.Libraries.Where(library => libraryIds.Contains(library.LibraryId)).ToListAsync();
        }

        public async Task<Library?> GetByName(string name)
        {
            return await _catalogContext.Libraries.Where(x => x.LibraryName == name).FirstOrDefaultAsync();
        }

        public async Task<List<Library>> GetByOwnerId(Guid ownerId)
        {
            return await _catalogContext.Libraries.Where(x => x.OwnerId == ownerId).ToListAsync();
        }

        public async Task<List<Guid>> GetLibraryIdsByOwnerId(Guid ownerId)
        {
            return await _catalogContext.Libraries.Where(x => x.OwnerId == ownerId).Select(x => x.LibraryId).ToListAsync();
        }

        public bool OwnerOfLibrary(Guid libraryId, Guid ownerId)
        {
            return _catalogContext.Libraries.Any(library => library.LibraryId == libraryId && library.OwnerId == ownerId);
        }

        public void UpdateLibrary(Library library)
        {
            _catalogContext.Libraries.Update(library);
        }
    }
}
