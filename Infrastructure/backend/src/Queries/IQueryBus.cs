using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public interface IQueryBus
    {
        Task<TResponse> Send<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>;
    }
}