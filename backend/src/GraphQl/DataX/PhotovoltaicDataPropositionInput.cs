using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public record PhotovoltaicDataPropositionInput(
        UuidPropositionInput? ComponentId,
        IReadOnlyList<PhotovoltaicDataPropositionInput>? And,
        PhotovoltaicDataPropositionInput? Not,
        IReadOnlyList<PhotovoltaicDataPropositionInput>? Or,
        GetHttpsResourcesPropositionInput? Resources
        );
}
