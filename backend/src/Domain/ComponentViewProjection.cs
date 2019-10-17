using Marten.Events.Projections;
using Guid = System.Guid;
using Marten;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Icon.Domain
{
    public sealed class ComponentViewProjection : ViewProjection<ComponentView, Guid>
    {private readonly ILogger _logger;
        public ComponentViewProjection(ILogger<ComponentViewProjection> logger)
        {
          _logger = logger;
            ProjectEvent<Component.Create.Event>(e => e.ComponentId, Apply);
            ProjectEventAsync<ComponentVersion.Create.Event>(e => e.ComponentId, Apply);
        }

        private void Apply(ComponentView view, Component.Create.Event @event)
        {
            view.Id = @event.ComponentId;
        }

        private async Task Apply(IDocumentSession documentSession, ComponentView view, ComponentVersion.Create.Event @event)
        {
          _logger.LogDebug("########");
          _logger.LogDebug(@event.ComponentVersionId);
            var version = await documentSession.LoadAsync<ComponentVersionView>(@event.ComponentVersionId);
            view.Versions.Add(version);
        }
    }
}