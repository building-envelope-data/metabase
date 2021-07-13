using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class OpticalData
    : Data
    {
        public OpticalData(
          // string id,
          Guid uuid,
          DateTime timestamp,
          // string locale,
          // Guid databaseId,
          Guid componentId,
          string? name,
          // string? description,
          // IReadOnlyList<string> warnings,
          // Guid creatorId,
          // DateTime createdAt,
          AppliedMethod appliedMethod,
          // IReadOnlyList<GetHttpsResource> resources,
          GetHttpsResourceTree resourceTree,
          // IReadOnlyList<DataApproval> approvals
          // ResponseApproval approval
          IReadOnlyList<double> nearnormalHemisphericalVisibleTransmittances
        ) : base(
          uuid: uuid,
          timestamp: timestamp,
          componentId: componentId,
          name: name,
          appliedMethod: appliedMethod,
          resourceTree: resourceTree
        )
        {
            NearnormalHemisphericalVisibleTransmittances = nearnormalHemisphericalVisibleTransmittances;
        }

        public IReadOnlyList<double> NearnormalHemisphericalVisibleTransmittances { get; }
    }
}
