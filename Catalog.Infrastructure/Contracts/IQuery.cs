using MediatR;

namespace Catalog.Infrastructure.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
