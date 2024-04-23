using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Guid = System.Guid;

namespace Metabase.GraphQl.Components;

[ExtendObjectType(nameof(Query))]
public sealed class ComponentQueries
{
    [UsePaging]
    // [UseProjection] // We disabled projections because when requesting `id` all results had the same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
    [UseFiltering]
    [UseSorting]
    public IQueryable<Data.Component> GetComponents(
        Data.ApplicationDbContext context
    )
    {
        return context.Components;
    }

    public Task<Data.Component?> GetComponentAsync(
        Guid uuid,
        ComponentByIdDataLoader componentById,
        CancellationToken cancellationToken
    )
    {
        return componentById.LoadAsync(
            uuid,
            cancellationToken
        );
    }
}