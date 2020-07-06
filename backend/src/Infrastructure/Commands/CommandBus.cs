using System.Threading.Tasks;
using MediatR;

namespace Icon.Infrastructure.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IMediator _mediator;

        public CommandBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task<TResponse> Send<TCommand, TResponse>(TCommand command) where TCommand : ICommand<TResponse>
        {
            return _mediator.Send(command);
        }
    }
}