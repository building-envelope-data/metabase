using System.Collections.Generic;
using Metabase.GraphQl.DataX;

namespace Metabase.GraphQl.LifeCycleDataX;

public sealed record LifeCycleDataPropositionInput(
    UuidPropositionInput? ComponentId,
    IReadOnlyList<LifeCycleDataPropositionInput>? And,
    LifeCycleDataPropositionInput? Not,
    IReadOnlyList<LifeCycleDataPropositionInput>? Or,
    GetHttpsResourcesPropositionInput? Resources
)
{
    public static readonly LifeCycleDataPropositionInput Empty =
        new(null, null, null, null, null);
};