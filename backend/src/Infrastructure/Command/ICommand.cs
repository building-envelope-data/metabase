using MediatR;
using Guid = System.Guid;

namespace Icon.Infrastructure.Command
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
        public Guid CreatorId { get; set; }
    }
}