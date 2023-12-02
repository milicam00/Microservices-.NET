namespace Catalog.Domain.OutboxMessages
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }

        public DateTime OccurredOn { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public OutboxMessage(DateTime occurredOn, string type, string data)
        {
            Id = Guid.NewGuid();
            this.OccurredOn = occurredOn;
            this.Type = type;
            this.Data = data;
        }
        public OutboxMessage(string type, string data)
        {
            Id = Guid.NewGuid();
            this.Type = type;
            this.Data = data;
            OccurredOn = DateTime.Now;
        }
        private OutboxMessage()
        {
        }

        public static OutboxMessage CreateMessage(string type, string data)
        {
            return new OutboxMessage(type, data);
        }
    }
}
