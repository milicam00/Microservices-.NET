namespace Catalog.Domain.OutboxMessages
{
    public interface IOutboxMessageRepository
    {
        Task AddAsync(OutboxMessage outbox);
        Task<List<OutboxMessage>> GetAllUnprocessedEmailMessages();
        Task AddAsync(List<OutboxMessage> outboxs);
        void Update(OutboxMessage outbox);


    }
}
