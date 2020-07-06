using MediatR;

namespace Icon.Infrastructure.Queries
{
    public interface IQuery<out TResponse>
      : IRequest<TResponse>
    {
    }
}