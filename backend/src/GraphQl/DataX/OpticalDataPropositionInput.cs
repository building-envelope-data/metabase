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
        FloatPropositionInput? InfraredEmittance
        // FloatPropositionInput? ColorRenderingIndex,
        // CielabColorPropositionInput? CielabColor
        );
}
