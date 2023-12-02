using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        public UnitOfWork(
            DbContext context)

        {
            this._context = context;

        }

        public async Task<int> CommitAsync(
            CancellationToken cancellationToken = default,
            Guid? internalCommandId = null)
        {

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
