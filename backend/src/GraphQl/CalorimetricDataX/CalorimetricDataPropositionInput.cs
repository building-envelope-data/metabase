using System.Collections.Generic;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.CalorimetricDataX;

public sealed record CalorimetricDataPropositionInput(
    UuidPropositionInput? ComponentId,
    IReadOnlyList<CalorimetricDataPropositionInput>? And,
    CalorimetricDataPropositionInput? Not,
    IReadOnlyList<CalorimetricDataPropositionInput>? Or,
    GetHttpsResourcesPropositionInput? Resources,
    FloatsPropositionInput? GValues,
    FloatsPropositionInput? UValues
);