using System;
using Marten.Events.Projections;
using Guid = System.Guid;
using Marten;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Icon.Domain
{
    public sealed class ComponentViewProjection : ViewProjection<ComponentView, Guid>
    {
        public ComponentViewProjection()
        {
            ProjectEvent<Component.Create.ComponentCreateEvent>(e => e.ComponentId, Apply);
            ProjectEventAsync<ComponentVersion.Create.ComponentVersionCreateEvent>(e => e.ComponentId, Apply);
        }

        private void Apply(ComponentView view, Component.Create.ComponentCreateEvent @event)
        {
            view.Id = @event.ComponentId;
        }

        private async Task Apply(IDocumentSession documentSession, ComponentView view, ComponentVersion.Create.ComponentVersionCreateEvent @event)
        {
            // TODO This relies on the order in which the views are persisted
            // in the database, which is the order in which the views are
            // registered in the event store configuration, which is error
            // prone. We should not rely on that! One option is to listen to
            // all component version events and build the component versions
            // here by hand or to add only the version identifiers and load
            // the corresponding objects later.
            var version = await documentSession.LoadAsync<ComponentVersionView>(@event.ComponentVersionId);
            view.Versions.Add(version);
        }
    }
}