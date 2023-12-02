using MediatR;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.Infrastructure.Configuration.Queries
{
    public interface IQueryHandler<in TQuery, TResult> :
        IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}
