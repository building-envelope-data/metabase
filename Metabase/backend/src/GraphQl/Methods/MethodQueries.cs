using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Guid = System.Guid;

namespace Metabase.GraphQl.Methods
{
    [ExtendObjectType(Name = nameof(Query))]
    public sealed class MethodQueries
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Method> GetMethods(
            [ScopedService] Data.ApplicationDbContext context
            )
        {
            return context.Methods;
        }

        public Task<Data.Method?> GetMethodAsync(
            Guid uuid,
            MethodByIdDataLoader methodById,
            CancellationToken cancellationToken
            )
        {
            return methodById.LoadAsync(
                uuid,
                cancellationToken
                );
        }
    }
}