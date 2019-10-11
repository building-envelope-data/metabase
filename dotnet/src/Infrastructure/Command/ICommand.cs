using MediatR;

namespace Icon.Infrastructure.Command
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}