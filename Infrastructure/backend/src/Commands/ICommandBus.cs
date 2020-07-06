using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public interface ICommandBus
    {
        Task<TResponse> Send<TCommand, TResponse>(TCommand command) where TCommand : ICommand<TResponse>;
    }
}