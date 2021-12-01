using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public record OpticalDataPropositionInput(
        UuidPropositionInput? ComponentId,
        IReadOnlyList<OpticalDataPropositionInput>? And,
        OpticalDataPropositionInput? Not,
        IReadOnlyList<OpticalDataPropositionInput>? Or,
        GetHttpsResourcesPropositionInput? Resources,
        FloatsPropositionInput? NearnormalHemisphericalVisibleTransmittances,
        FloatsPropositionInput? NearnormalHemisphericalVisibleReflectances,
        FloatsPropositionInput? NearnormalHemisphericalSolarTransmittances,
        FloatsPropositionInput? NearnormalHemisphericalSolarReflectances,
        FloatsPropositionInput? InfraredEmittances,
        FloatsPropositionInput? ColorRenderingIndices,
        CielabColorsPropositionInput? CielabColors
        );
}
