namespace Catalog.Domain.Libraries
{
    public interface ILibraryRepository
    {
        Task AddAsync(Library library);
        Task<Library?> GetByIdAsync(Guid libraryId);
        Task<List<Library>> GetByIdsAsync(List<Guid> libraryIds);
        Task<List<Library>> GetByOwnerId(Guid ownerId);
        Task<List<Guid>> GetLibraryIdsByOwnerId(Guid ownerId);
        void UpdateLibrary(Library library);
        Task<Library> GetByName(string name);
        bool OwnerOfLibrary(Guid libraryId, Guid ownerId);
    }
}
