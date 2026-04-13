using System;
using System.Collections.Generic;
using NodaTime;

namespace Metabase.GraphQl.DataX;

public sealed class PhotovoltaicData(
    string id,
    Guid uuid,
    OffsetDateTime timestamp,
    string locale,
    Guid databaseId,
    Guid componentId,
    string? name,
    string? description,
    IReadOnlyList<string> warnings,
    Guid creatorId,
    OffsetDateTime createdAt,
    AppliedMethod appliedMethod,
    IReadOnlyList<GetHttpsResource> resources,
    GetHttpsResourceTree resourceTree,
    IReadOnlyList<DataApproval> approvals
    // ResponseApproval approval
    )
        : Data(
    id,
    uuid,
    timestamp,
    locale,
    databaseId,
    componentId,
    name,
    description,
    warnings,
    creatorId,
    createdAt,
    appliedMethod,
    resources,
    resourceTree,
    approvals
    )
{
    public override DataKind Kind { get => DataKind.PHOTOVOLTAIC_DATA; }
}