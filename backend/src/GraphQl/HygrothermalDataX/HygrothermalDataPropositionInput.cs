using System.Collections.Generic;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.HygrothermalDataX;

public sealed record HygrothermalDataPropositionInput(
    UuidPropositionInput? ComponentId,
    IReadOnlyList<HygrothermalDataPropositionInput>? And,
    HygrothermalDataPropositionInput? Not,
    IReadOnlyList<HygrothermalDataPropositionInput>? Or,
    GetHttpsResourcesPropositionInput? Resources
);