using Icon;
using MediatR;
using Guid = System.Guid;

namespace Icon.Infrastructure.Command
{
    public interface ICommand<out TResponse>
      : IRequest<TResponse>, IValidatable
    {
        public Guid CreatorId { get; }
    }
}