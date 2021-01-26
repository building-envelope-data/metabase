using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Guid = System.Guid;

namespace Metabase.GraphQl.Components
{
    [ExtendObjectType(Name = nameof(Query))]
    public sealed class ComponentQueries
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Component> GetComponents(
            [ScopedService] Data.ApplicationDbContext context
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
}