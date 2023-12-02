using MediatR;

namespace UserAccess.Infrastructure.Contracts
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
