using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public record DataPropositionInput(
        UuidPropositionInput? ComponentId,
        IReadOnlyList<DataPropositionInput>? And,
        FloatPropositionInput? GValue,
        FloatPropositionInput? NearnormalHemisphericalVisibleTransmittance,
        DataPropositionInput? Not,
        IReadOnlyList<DataPropositionInput>? Or,
        FloatPropositionInput? UValue
        );
}
