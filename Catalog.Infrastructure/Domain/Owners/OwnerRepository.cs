using Catalog.Domain.Owners;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Domain.Owners
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly CatalogContext _catalogContext;
        public OwnerRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }
        public async Task AddAsync(Owner owner)
        {
            await _catalogContext.AddAsync(owner);
        }

        public void Delete(Owner owner)
        {
            _catalogContext.Owners.Remove(owner);
        }

        public async Task<Owner?> GetById(Guid ownerId)
        {
            return await _catalogContext.Owners.FirstOrDefaultAsync(x => x.OwnerId == ownerId);
        }

        public async Task<Owner?> GetByUsername(string username)
        {
            return await _catalogContext.Owners.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public void Update(Owner owner)
        {
            _catalogContext.Owners.Update(owner);
        }

        public void UpdateOwner(Owner owner)
        {
            _catalogContext.Owners.Update(owner);
        }

    }
}
