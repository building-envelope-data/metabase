using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public record CalorimetricDataPropositionInput(
        UuidPropositionInput? ComponentId,
        IReadOnlyList<CalorimetricDataPropositionInput>? And,
        FloatPropositionInput? GValue,
        FloatsPropositionInput? GValues,
        CalorimetricDataPropositionInput? Not,
        IReadOnlyList<CalorimetricDataPropositionInput>? Or,
        FloatPropositionInput? UValue,
        FloatsPropositionInput? UValues,
        UuidPropositionInput? DataFormatId
        );
}
