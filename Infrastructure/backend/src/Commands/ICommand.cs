using Infrastructure.ValueObjects;
using MediatR;

namespace Infrastructure.Commands
{
    public interface ICommand<out TResponse>
      : IRequest<TResponse>
    {
        public Id CreatorId { get; }
    }
}