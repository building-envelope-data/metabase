using System;
using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.Components;
using System.Collections.Generic;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.DataX;

public abstract class Data
    : IData
{
    public string Id { get; }
    public Guid Uuid { get; }
    public DateTime Timestamp { get; }

    public string Locale { get; }

    // public Guid DatabaseId { get; }
    public Guid ComponentId { get; }
    public string? Name { get; }
    public string? Description { get; }
    public IReadOnlyList<string> Warnings { get; }
    public Guid CreatorId { get; }
    public DateTime CreatedAt { get; }
    public AppliedMethod AppliedMethod { get; }
    public IReadOnlyList<GetHttpsResource> Resources { get; }

    public GetHttpsResourceTree ResourceTree { get; }
    // public IReadOnlyList<DataApproval> Approvals { get; }
    // public ResponseApproval Approval { get; }

    // [GraphQLIgnore]
    // [JsonExtensionData]
    // public Dictionary<string, JsonElement>? ExtensionData { get; set; }

    protected Data(
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
        GetHttpsResourceTree resourceTree
        // IReadOnlyList<DataApproval> approvals
        // ResponseApproval approval
    )
    {
        Id = id;
        Uuid = uuid;
        Timestamp = timestamp;
        Locale = locale;
        // DatabaseId = databaseId;
        ComponentId = componentId;
        Name = name;
        Description = description;
        Warnings = warnings;
        CreatorId = creatorId;
        CreatedAt = createdAt;
        AppliedMethod = appliedMethod;
        Resources = resources;
        ResourceTree = resourceTree;
        // Approvals = approvals;
        // Approval = approval;
    }

    // public Task<Metabase.Data.Database?> GetDatabaseAsync(
    //         DatabaseByIdDataLoader databaseById,
    //         CancellationToken cancellationToken
    // )
    // {
    //     return databaseById.LoadAsync(
    //         DatabaseId,
    //         cancellationToken
    //         );
    // }

    public Task<Metabase.Data.Component?> GetComponentAsync(
        ComponentByIdDataLoader componentById,
        CancellationToken cancellationToken
    )
    {
        return componentById.LoadAsync(
            ComponentId,
            cancellationToken
        );
    }

    public Task<Metabase.Data.Institution?> GetCreatorAsync(
        InstitutionByIdDataLoader institutionById,
        CancellationToken cancellationToken
    )
    {
        return institutionById.LoadAsync(
            CreatorId,
            cancellationToken
        );
    }
}