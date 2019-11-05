using System;
using Marten.Events.Projections;
using Marten;
using System.Threading.Tasks;
using System.Collections.Generic;
using Events = Icon.Events;
using Models = Icon.Models;
using Aggregates = Icon.Aggregates;

namespace Icon.Projections
{
    public sealed class ComponentVersionsProjection
      : ViewProjection<List<Models.ComponentVersion>, Guid>
    {
        /* private readonly List<Models.ComponentVersion> versions; */
        /* public IEnumerable<Models.ComponentVersion> versions { get { return versions.AsReadOnly(); } } */

        public ComponentVersionsProjection()
        {
            ProjectEventAsync<Events.ComponentVersionCreated>(e => e.ComponentId, Apply);
        }

        private async Task Apply(
            IDocumentSession documentSession,
            List<Models.ComponentVersion> view,
            Events.ComponentVersionCreated @event
            )
        {
            view.Add((await documentSession.LoadAsync<Aggregates.ComponentVersionAggregate>(@event.ComponentVersionId)).ToModel());
        }
    }
}