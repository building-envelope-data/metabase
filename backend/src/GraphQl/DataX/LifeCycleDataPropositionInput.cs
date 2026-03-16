using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed record LifeCycleDataPropositionInput(
    UuidPropositionInput? ComponentId,
    IReadOnlyList<LifeCycleDataPropositionInput>? And,
    LifeCycleDataPropositionInput? Not,
    IReadOnlyList<LifeCycleDataPropositionInput>? Or,
    GetHttpsResourcesPropositionInput? Resources
);