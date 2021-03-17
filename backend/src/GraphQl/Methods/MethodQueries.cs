using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
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
        // [UseProjection] // We disabled projections because when requesting `id` all results had the same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
        [UseFiltering]
        [UseSorting]
        [Authorize(Policy = Configuration.AuthConfiguration.ReadPolicy)]
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