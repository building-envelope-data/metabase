using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentType
      : ObjectType<Data.Component>
    {
      protected override void Configure(
          IObjectTypeDescriptor<Data.Component> descriptor
          )
        {
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((context, id) =>
                    context
                    .DataLoader<GraphQl.Components.ComponentByIdDataLoader>()
                    .LoadAsync(id, context.RequestAborted)
                    );

            // TODO Do we want to expose this, require it as input, and use it to discover concurrent writes?
            descriptor
              .Field(t => t.xmin)
              .Name("version")
              .Ignore();

            /* descriptor */
            /*     .Field(t => t.SessionsComponents) */
            /*     .ResolveWith<ComponentResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default)) */
            /*     .UseDbContext<ApplicationDbContext>() */
            /*     .Name("sessions"); */
        }

        /* private class ComponentResolvers */
        /* { */
        /*     public async Task<IEnumerable<Session>> GetSessionsAsync( */
        /*         Data.Component component, */
        /*         [ScopedService] ApplicationDbContext dbContext, */
        /*         SessionByIdDataLoader sessionById, */
        /*         CancellationToken cancellationToken) */
        /*     { */
        /*         int[] speakerIds = await dbContext.Components */
        /*             .Where(a => a.Id == component.Id) */
        /*             .Include(a => a.SessionsComponents) */
        /*             .SelectMany(a => a.SessionsComponents.Select(t => t.SessionId)) */
        /*             .ToArrayAsync(); */

        /*         return await sessionById.LoadAsync(speakerIds, cancellationToken); */
        /*     } */
        /* } */
    }
}
