using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class CalorimetricData
    : Data
{
    public CalorimetricData(
        string id,
        Guid uuid,
        DateTime timestamp,
        string locale,
        // Guid databaseId,
        Guid componentId,
        string? name,
        string? description,
        IReadOnlyList<string> warnings,
        Guid creatorId,
        DateTime createdAt,
        AppliedMethod appliedMethod,
        IReadOnlyList<GetHttpsResource> resources,
        GetHttpsResourceTree resourceTree,
        // IReadOnlyList<DataApproval> approvals
        // ResponseApproval approval
        IReadOnlyList<double> gValues,
        IReadOnlyList<double> uValues
    ) : base(
        id,
        uuid,
        timestamp,
        locale,
        componentId,
        name,
        description,
        warnings,
        creatorId,
        createdAt,
        appliedMethod,
        resources,
        resourceTree
    )
    {
        GValues = gValues;
        UValues = uValues;
    }

    public IReadOnlyList<double> GValues { get; }
    public IReadOnlyList<double> UValues { get; }
}