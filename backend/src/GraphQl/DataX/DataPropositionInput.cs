using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public record DataPropositionInput(
        UuidPropositionInput? ComponentId,
        IReadOnlyList<DataPropositionInput>? And,
        DataPropositionInput? Not,
        IReadOnlyList<DataPropositionInput>? Or,
        UuidPropositionInput? DataFormatId,
        FloatPropositionInput? GValue,
        FloatPropositionInput? UValue,
        FloatPropositionInput? NearnormalHemisphericalVisibleTransmittance,
        FloatPropositionInput? NearnormalHemisphericalVisibleReflectance,
        FloatPropositionInput? NearnormalHemisphericalSolarTransmittance,
        FloatPropositionInput? NearnormalHemisphericalSolarReflectance,
        FloatPropositionInput? InfraredEmittance,
        FloatPropositionInput? ColorRenderingIndex,
        CielabColorPropositionInput? CielabColor,
        FloatsPropositionInput? GValues,
        FloatsPropositionInput? UValues,
        FloatsPropositionInput? NearnormalHemisphericalVisibleTransmittances,
        FloatsPropositionInput? NearnormalHemisphericalVisibleReflectances,
        FloatsPropositionInput? NearnormalHemisphericalSolarTransmittances,
        FloatsPropositionInput? NearnormalHemisphericalSolarReflectances,
        FloatsPropositionInput? InfraredEmittances,
        FloatsPropositionInput? ColorRenderingIndices,
        CielabColorsPropositionInput? CielabColors
        );
}
