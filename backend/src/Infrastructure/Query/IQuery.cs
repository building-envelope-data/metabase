using MediatR;

namespace Icon.Infrastructure.Query
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}