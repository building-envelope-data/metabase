using Icon;
using MediatR;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Infrastructure.Command
{
    public interface ICommand<out TResponse>
      : IRequest<TResponse>
    {
        public ValueObjects.Id CreatorId { get; }
    }
}