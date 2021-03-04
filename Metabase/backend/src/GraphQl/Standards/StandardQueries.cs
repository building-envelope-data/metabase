using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Guid = System.Guid;

namespace Metabase.GraphQl.Standards
{
    [ExtendObjectType(Name = nameof(Query))]
    public sealed class StandardQueries
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging]
        // [UseProjection] // We disabled projections because when requesting `id` all results had the same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
        [UseFiltering]
        [UseSorting]
        [Authorize(Policy = Configuration.Auth.ReadPolicy)]
        public IQueryable<Data.Standard> GetStandards(
            [ScopedService] Data.ApplicationDbContext context
            )
        {
            return context.Standards;
        }

        public Task<Data.Standard?> GetStandardAsync(
            Guid uuid,
            StandardByIdDataLoader standardById,
            CancellationToken cancellationToken
            )
        {
            return standardById.LoadAsync(
                uuid,
                cancellationToken
                );
        }
    }
}