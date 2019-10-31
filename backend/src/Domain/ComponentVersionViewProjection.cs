using System;
using Marten.Events.Projections;
using Guid = System.Guid;
using Marten;
using System.Threading.Tasks;

namespace Icon.Domain
{
    public sealed class ComponentVersionViewProjection : ViewProjection<ComponentVersionView, Guid>
    {
        public ComponentVersionViewProjection()
        {
            ProjectEvent<ComponentVersion.Create.ComponentVersionCreateEvent>(e => e.ComponentVersionId, Apply);
            ProjectEventAsync<ComponentVersionOwnership.Create.ComponentVersionOwnershipEvent>(e => e.ComponentVersionId, Apply);
        }

        private void Apply(ComponentVersionView view, ComponentVersion.Create.ComponentVersionCreateEvent @event)
        {
            view.Id = @event.ComponentVersionId;
            view.ComponentId = @event.ComponentId;
        }

        private async Task Apply(IDocumentSession session, ComponentVersionView view, ComponentVersionOwnership.Create.ComponentVersionOwnershipEvent @event)
        {
            // TODO This relies on the order in which the views are persisted
            // in the database, which is the order in which the views are
            // registered in the event store configuration, which is error
            // prone. We should not rely on that! One option is to listen to
            // all component version ownership events and build the component
            // version ownerships here by hand or to add only the ownership
            // identifiers and load the corresponding objects later.
            var ownership = await session.LoadAsync<ComponentVersionOwnershipView>(@event.ComponentVersionOwnershipId);
            view.Ownerships.Add(ownership);
        }
    }
}