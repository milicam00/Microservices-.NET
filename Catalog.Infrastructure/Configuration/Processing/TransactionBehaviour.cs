using BuildingBlocks.Infrastructure;
using MediatR;
using System.Transactions;

namespace Catalog.Infrastructure.Configuration.Processing
{
    public static class GenericTypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            string typeName;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object @object)
        {
            return @object.GetType().GetGenericTypeName();
        }
    }

    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionBehaviour(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(IUnitOfWork));
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (IsNotCommand())
                return await next();

            var typeName = request.GetGenericTypeName();
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    string transactionId = string.Empty;
                    if (Transaction.Current is not null)
                    {
                        TransactionInformation info = Transaction.Current.TransactionInformation;
                        transactionId = Guid.Empty.Equals(info.DistributedIdentifier) ? info.LocalIdentifier : info.DistributedIdentifier.ToString();
                    }
                    var response = await next();
                    await _unitOfWork.CommitAsync(cancellationToken);
                    transactionScope.Complete();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static bool IsNotCommand()
        {
            return !typeof(TRequest).Name.EndsWith("Command");
        }

    }
}
