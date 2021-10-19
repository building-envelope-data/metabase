using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public record OpticalDataPropositionInput(
        UuidPropositionInput? ComponentId,
        IReadOnlyList<OpticalDataPropositionInput>? And,
        OpticalDataPropositionInput? Not,
        IReadOnlyList<OpticalDataPropositionInput>? Or,
        // UuidPropositionInput? DataFormatId,
        FloatPropositionInput? NearnormalHemisphericalVisibleTransmittance,
        FloatPropositionInput? NearnormalHemisphericalVisibleReflectance,
        FloatPropositionInput? NearnormalHemisphericalSolarTransmittance,
        FloatPropositionInput? NearnormalHemisphericalSolarReflectance,
        FloatPropositionInput? InfraredEmittance,
        FloatPropositionInput? ColorRenderingIndex,
        CielabColorPropositionInput? CielabColor,
        FloatsPropositionInput? NearnormalHemisphericalVisibleTransmittances,
        FloatsPropositionInput? NearnormalHemisphericalVisibleReflectances,
        FloatsPropositionInput? NearnormalHemisphericalSolarTransmittances,
        FloatsPropositionInput? NearnormalHemisphericalSolarReflectances,
        FloatsPropositionInput? InfraredEmittances,
        FloatsPropositionInput? ColorRenderingIndices,
        CielabColorsPropositionInput? CielabColors
        );
}
