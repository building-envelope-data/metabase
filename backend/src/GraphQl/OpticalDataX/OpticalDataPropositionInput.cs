using System.Collections.Generic;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.OpticalDataX;

public sealed record OpticalDataPropositionInput(
    UuidPropositionInput? ComponentId,
    IReadOnlyList<OpticalDataPropositionInput>? And,
    OpticalDataPropositionInput? Not,
    IReadOnlyList<OpticalDataPropositionInput>? Or,
    GetHttpsResourcesPropositionInput? Resources,
    OpticalComponentTypePropositionInput? Type,
    OpticalComponentSubtypePropositionInput? Subtype,
    CoatedSidePropositionInput? CoatedSide,
    FloatsPropositionInput? NearnormalHemisphericalVisibleTransmittances,
    FloatsPropositionInput? NearnormalHemisphericalVisibleReflectances,
    FloatsPropositionInput? NearnormalHemisphericalSolarTransmittances,
    FloatsPropositionInput? NearnormalHemisphericalSolarReflectances,
    FloatsPropositionInput? InfraredEmittances,
    FloatsPropositionInput? ColorRenderingIndices,
    CielabColorsPropositionInput? CielabColors
);