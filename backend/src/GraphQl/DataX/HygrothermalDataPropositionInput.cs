using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed record HygrothermalDataPropositionInput(
        UuidPropositionInput? ComponentId,
        IReadOnlyList<HygrothermalDataPropositionInput>? And,
        HygrothermalDataPropositionInput? Not,
        IReadOnlyList<HygrothermalDataPropositionInput>? Or,
        GetHttpsResourcesPropositionInput? Resources
    );
}