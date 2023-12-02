using Catalog.Domain.Readers;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Domain.Readers
{
    public class ReaderRepository : IReaderRepository
    {
        private readonly CatalogContext _catalogContext;
        public ReaderRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }
        public async Task AddAsync(Reader reader)
        {
            await _catalogContext.AddAsync(reader);
        }

        public void Delete(Reader reader)
        {
            _catalogContext.Readers.Remove(reader);
        }

        public async Task<Reader?> GetById(Guid readerId)
        {
            return await _catalogContext.Readers.FirstOrDefaultAsync(x => x.ReaderId == readerId);
        }

        public async Task<Reader?> GetByUsername(string username)
        {
            return await _catalogContext.Readers.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public void Update(Reader reader)
        {
            _catalogContext.Readers.Update(reader);
        }
    }
}
