using System;
using System.Collections.Generic;
using NodaTime;

namespace Metabase.GraphQl.DataX;

public sealed class CalorimetricData(
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
    IReadOnlyList<DataApproval> approvals,
    // ResponseApproval approval
    IReadOnlyList<double> gValues,
    IReadOnlyList<double> uValues
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
    public override DataKind Kind { get => DataKind.CALORIMETRIC_DATA; }
    public IReadOnlyList<double> GValues { get; } = gValues;
    public IReadOnlyList<double> UValues { get; } = uValues;
}