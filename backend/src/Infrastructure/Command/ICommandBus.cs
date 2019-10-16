using System.Threading.Tasks;

namespace Icon.Infrastructure.Command
{
    public interface ICommandBus
    {
        Task<TResponse> Send<TCommand, TResponse>(TCommand command) where TCommand : ICommand<TResponse>;
    }
}