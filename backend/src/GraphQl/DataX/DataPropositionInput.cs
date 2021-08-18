using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public record DataPropositionInput(
        UuidPropositionInput? ComponentId,
        IReadOnlyList<DataPropositionInput>? And,
        DataPropositionInput? Not,
        IReadOnlyList<DataPropositionInput>? Or,
        FloatPropositionInput? GValue,
        FloatPropositionInput? UValue,
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
