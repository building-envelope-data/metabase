using MediatR;

namespace Icon.Infrastructure.Commands
{
    public interface ICommand<out TResponse>
      : IRequest<TResponse>
    {
        public ValueObjects.Id CreatorId { get; }
    }
}