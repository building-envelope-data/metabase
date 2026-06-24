using System.Collections.Generic;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.GeometricDataX;

public sealed record GeometricDataPropositionInput(
    UuidPropositionInput? ComponentId,
    IReadOnlyList<GeometricDataPropositionInput>? And,
    GeometricDataPropositionInput? Not,
    IReadOnlyList<GeometricDataPropositionInput>? Or,
    GetHttpsResourcesPropositionInput? Resources,
    FloatsPropositionInput? Thicknesses
)
{
    public static readonly GeometricDataPropositionInput Empty =
        new(null, null, null, null, null, null);
};