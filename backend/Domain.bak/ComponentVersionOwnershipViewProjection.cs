using Marten.Events.Projections;
using Guid = System.Guid;
using Marten;
using System.Threading.Tasks;

namespace Icon.Domain
{
    public sealed class ComponentVersionManufacturerViewProjection : ViewProjection<ComponentVersionManufacturerView, Guid>
    {
        public ComponentVersionManufacturerViewProjection()
        {
            ProjectEvent<ComponentVersionManufacturer.Create.ComponentVersionManufacturerEvent>(e => e.ComponentVersionId, Apply);
        }

        private void Apply(ComponentVersionManufacturerView view, ComponentVersionManufacturer.Create.ComponentVersionManufacturerEvent @event)
        {
            view.Id = @event.ComponentVersionManufacturerId;
            view.ComponentVersionId = @event.ComponentVersionId;
            view.Name = @event.Data.Name;
            view.Description = @event.Data.Description;
            view.AvailableFrom = @event.Data.AvailableFrom;
            view.AvailableUntil = @event.Data.AvailableUntil;
        }
    }
}