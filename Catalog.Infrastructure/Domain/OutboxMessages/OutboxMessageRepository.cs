using Catalog.Domain.OutboxMessages;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Domain.OutboxMessages
{
    public class OutboxMessageRepository : IOutboxMessageRepository
    {
        private readonly CatalogContext _catalogContext;

        public OutboxMessageRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task AddAsync(OutboxMessage message)
        {
            await _catalogContext.OutboxMessages.AddAsync(message);
        }

        public async Task AddAsync(List<OutboxMessage> outboxs)
        {
            foreach (var outbox in outboxs)
            {
                await _catalogContext.OutboxMessages.AddAsync(outbox);
            }
        }

        public async Task<List<OutboxMessage>> GetAllUnprocessedEmailMessages()
        {
            return await _catalogContext.OutboxMessages.Where(x => x.ProcessedDate == null).ToListAsync();
        }

        public void Update(OutboxMessage outbox)
        {
            _catalogContext.OutboxMessages.Update(outbox);
            _catalogContext.SaveChangesAsync();
        }
    }
}
