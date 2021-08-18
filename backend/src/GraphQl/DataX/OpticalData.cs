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
          string? description,
          // IReadOnlyList<string> warnings,
          // Guid creatorId,
          // DateTime createdAt,
          AppliedMethod appliedMethod,
          // IReadOnlyList<GetHttpsResource> resources,
          GetHttpsResourceTree resourceTree,
          // IReadOnlyList<DataApproval> approvals
          // ResponseApproval approval
          IReadOnlyList<double> nearnormalHemisphericalVisibleTransmittances,
          IReadOnlyList<double> nearnormalHemisphericalVisibleReflectances,
          IReadOnlyList<double> nearnormalHemisphericalSolarTransmittances,
          IReadOnlyList<double> nearnormalHemisphericalSolarReflectances,
          IReadOnlyList<double> infraredEmittances
        // IReadOnlyList<double> colorRenderingIndices,
        // IReadOnlyList<CielabColor> cielabColors
        ) : base(
          uuid: uuid,
          timestamp: timestamp,
          componentId: componentId,
          name: name,
          description: description,
          appliedMethod: appliedMethod,
          resourceTree: resourceTree
        )
        {
            NearnormalHemisphericalVisibleTransmittances = nearnormalHemisphericalVisibleTransmittances;
            NearnormalHemisphericalVisibleReflectances = nearnormalHemisphericalVisibleReflectances;
            NearnormalHemisphericalSolarTransmittances = nearnormalHemisphericalSolarTransmittances;
            NearnormalHemisphericalSolarReflectances = nearnormalHemisphericalSolarReflectances;
            InfraredEmittances = infraredEmittances;
            // ColorRenderingIndices = colorRenderingIndices;
            // CielabColors = cielabColors;
        }

        public IReadOnlyList<double> NearnormalHemisphericalVisibleTransmittances { get; }
        public IReadOnlyList<double> NearnormalHemisphericalVisibleReflectances { get; }
        public IReadOnlyList<double> NearnormalHemisphericalSolarTransmittances { get; }
        public IReadOnlyList<double> NearnormalHemisphericalSolarReflectances { get; }
        public IReadOnlyList<double> InfraredEmittances { get; }
        // public IReadOnlyList<double> ColorRenderingIndices { get; }
        // public IReadOnlyList<CielabColor> CielabColors { get; }
    }
}
