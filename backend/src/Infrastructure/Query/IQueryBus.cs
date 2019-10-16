using System.Threading.Tasks;

namespace Icon.Infrastructure.Query
{
    public interface IQueryBus
    {
        Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>;
    }
}