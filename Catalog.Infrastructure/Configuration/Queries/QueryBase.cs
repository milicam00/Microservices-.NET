using Catalog.Infrastructure.Contracts;

namespace Catalog.Infrastructure.Configuration.Queries
{
    public abstract class QueryBase<TResult> : IQuery<TResult>
    {
        public Guid Id { get; }

        protected QueryBase()
        {
            this.Id = Guid.NewGuid();
        }

        protected QueryBase(Guid id)
        {
            this.Id = id;
        }
    }
}
