using Guid = System.Guid;
using System.Collections.Generic;
using Models = Icon.Models;
using Icon.Infrastructure.Query;

namespace Icon.Queries
{
    public class ListComponentVersions :
      IQuery<IEnumerable<Models.ComponentVersion>>
    {
        public Guid ComponentId { get; }

        public ListComponentVersions(
            Guid componentId
            )
        {
            ComponentId = componentId;
        }
    }
}
