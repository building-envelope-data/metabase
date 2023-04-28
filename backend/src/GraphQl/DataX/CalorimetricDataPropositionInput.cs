using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed record CalorimetricDataPropositionInput(
        UuidPropositionInput? ComponentId,
        IReadOnlyList<CalorimetricDataPropositionInput>? And,
        CalorimetricDataPropositionInput? Not,
        IReadOnlyList<CalorimetricDataPropositionInput>? Or,
        GetHttpsResourcesPropositionInput? Resources,
        FloatsPropositionInput? GValues,
        FloatsPropositionInput? UValues
        );
}
