using System.Collections.Generic;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.PhotovoltaicDataX;

public sealed record PhotovoltaicDataPropositionInput(
    UuidPropositionInput? ComponentId,
    IReadOnlyList<PhotovoltaicDataPropositionInput>? And,
    PhotovoltaicDataPropositionInput? Not,
    IReadOnlyList<PhotovoltaicDataPropositionInput>? Or,
    GetHttpsResourcesPropositionInput? Resources
)
{
    public static readonly PhotovoltaicDataPropositionInput Empty =
        new(null, null, null, null, null);
};