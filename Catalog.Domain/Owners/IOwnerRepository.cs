namespace Catalog.Domain.Owners
{
    public interface IOwnerRepository
    {
        Task AddAsync(Owner owner);
        void UpdateOwner(Owner owner);
        Task<Owner> GetById(Guid ownerId);
        Task<Owner> GetByUsername(string username);
        void Update(Owner owner);
        void Delete(Owner owner);

    }
}
