using UserAccess.Infrastructure.Contracts;

namespace UserAccess.Infrastructure.Configuration.Commands
{
    public interface ICommandsScheduler
    {
        Task EnqueueAsync(ICommand command);
        Task EnqueueAsync<T>(ICommand<T> command);
    }
}
