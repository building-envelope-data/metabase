using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace Metabase.GraphQl.Components
{
    public sealed class ComponentType
      : EntityType<Data.Component, ComponentByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Component> descriptor
            )
        {
            base.Configure(descriptor);
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
