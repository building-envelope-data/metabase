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
            ProjectEvent<ComponentVersionOwnership.Create.ComponentVersionOwnershipEvent>(e => e.ComponentVersionId, Apply);
        }

        private void Apply(ComponentVersionOwnershipView view, ComponentVersionOwnership.Create.ComponentVersionOwnershipEvent @event)
        {
            view.Id = @event.ComponentVersionOwnershipId;
            view.ComponentVersionId = @event.ComponentVersionId;
            view.Name = @event.Data.Name;
            view.Description = @event.Data.Description;
            view.AvailableFrom = @event.Data.AvailableFrom;
            view.AvailableUntil = @event.Data.AvailableUntil;
        }
    }
}