using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Guid = System.Guid;

namespace Metabase.GraphQl.Components
{
    [ExtendObjectType(Name = nameof(GraphQl.Query))]
    public sealed class ComponentQueries
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging]
        /* TODO [UseProjection] // fails without an explicit error message in the logs */
        /* TODO [UseFiltering(typeof(ComponentFilterType))] // wait for https://github.com/ChilliCream/hotchocolate/issues/2672 and https://github.com/ChilliCream/hotchocolate/issues/2666 */
        [UseSorting]
        public IQueryable<Data.Component> GetComponents(
            [ScopedService] Data.ApplicationDbContext context
            )
        {
            return context.Components;
        }

        /* public Task<IEnumerable<Component>> GetComponents( */
        /*     [ID(nameof(Data.Component))] Guid[] ids, */
        /*     ComponentByIdDataLoader componentById, */
        /*     CancellationToken cancellationToken) */
        /* { */
        /*   return componentById.LoadAsync( */
        /*       ids, */
        /*       cancellationToken */
        /*       ); */
        /* } */

        public Task<Data.Component?> GetComponentAsync(
            [ID(nameof(Data.Component))] Guid id,
            ComponentByIdDataLoader componentById,
            CancellationToken cancellationToken
            )
        {
            return componentById.LoadAsync(
                id,
                cancellationToken
                );
        }
    }
}
