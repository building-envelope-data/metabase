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
            ProjectEvent<ComponentVersion.Create.Event>(e => e.ComponentVersionId, Apply);
            ProjectEventAsync<ComponentVersionOwnership.Create.Event>(e => e.ComponentVersionId, Apply);
        }

        private void Apply(ComponentVersionView view, ComponentVersion.Create.Event @event)
        {
            view.Id = @event.ComponentVersionId;
            view.ComponentId = @event.ComponentId;
        }

        private async Task Apply(IDocumentSession session, ComponentVersionView view, ComponentVersionOwnership.Create.Event @event)
        {
            var ownership = await session.LoadAsync<ComponentVersionOwnershipView>(@event.ComponentVersionOwnershipId);
            view.Ownerships.Add(ownership);
        }
    }
}