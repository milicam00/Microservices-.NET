using Catalog.Infrastructure.Contracts;
using MediatR;

namespace Catalog.Infrastructure.Configuration.Queries
{

    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}
