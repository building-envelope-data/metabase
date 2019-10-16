using Marten.Events.Projections;
using Guid = System.Guid;
using Marten;
using System.Threading.Tasks;

namespace Icon.Domain
{
    public sealed class ComponentVersionOwnershipViewProjection : ViewProjection<ComponentVersionOwnershipView, Guid>
    {
        public ComponentVersionOwnershipViewProjection()
        {
            ProjectEvent<ComponentVersionOwnership.Create.Event>(e => e.ComponentVersionId, Apply);
        }

        private void Apply(ComponentVersionOwnershipView view, ComponentVersionOwnership.Create.Event @event)
        {
            view.Id = @event.ComponentVersionOwnershipId;
            view.ComponentVersionId = @event.ComponentVersionId;
        }
    }
}