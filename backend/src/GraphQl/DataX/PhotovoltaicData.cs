using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX;

public sealed class PhotovoltaicData
    : Data
{
    public PhotovoltaicData(
        string id,
        Guid uuid,
        DateTime timestamp,
        string locale,
        Guid databaseId,
        Guid componentId,
        string? name,
        string? description,
        IReadOnlyList<string> warnings,
        Guid creatorId,
        DateTime createdAt,
        AppliedMethod appliedMethod,
        IReadOnlyList<GetHttpsResource> resources,
        GetHttpsResourceTree resourceTree
        // IReadOnlyList<DataApproval> approvals
        // ResponseApproval approval
    ) : base(
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
        resourceTree
    )
    {
    }
}