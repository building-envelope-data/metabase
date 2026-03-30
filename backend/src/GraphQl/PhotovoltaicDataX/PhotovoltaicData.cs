using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types.Relay;
using Metabase.Data;
using Metabase.GraphQl.DataX;
using Metabase.GraphQl.Requests;
using NodaTime;

namespace Metabase.GraphQl.PhotovoltaicDataX;

[Node(IdField = nameof(Id))]
public sealed record PhotovoltaicData(
    string Id,
    Guid Uuid,
    OffsetDateTime Timestamp,
    string Locale,
    Guid DatabaseId,
    Guid ComponentId,
    string? Name,
    string? Description,
    IReadOnlyList<string> Warnings,
    Guid CreatorId,
    OffsetDateTime CreatedAt,
    AppliedMethod AppliedMethod,
    IReadOnlyList<GetHttpsResource> Resources,
    GetHttpsResourceTree ResourceTree,
    IReadOnlyList<DataApproval> Approvals
// ResponseApproval Approval
)
: DataX.Data(
    Id,
    Uuid,
    Timestamp,
    Locale,
    DatabaseId,
    ComponentId,
    Name,
    Description,
    Warnings,
    CreatorId,
    CreatedAt,
    AppliedMethod,
    Resources,
    ResourceTree,
    Approvals
)
{
    public override DataKind Kind { get => DataKind.PHOTOVOLTAIC_DATA; }

    [NodeResolver]
    public static Task<PhotovoltaicData?> GetAsync(
        string id,
        DataQueries dataQueries,
        ApplicationDbContext databaseContext,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return DataX.Data.FetchNodeAsync<PhotovoltaicData>(
            id,
            (database, uuid, locale) => dataQueries.GetPhotovoltaicDataAsync(
                database,
                uuid,
                locale,
                resolverContext,
                cancellationToken
            ),
            databaseContext,
            cancellationToken
        );
    }
}