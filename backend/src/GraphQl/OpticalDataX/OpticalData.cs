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

namespace Metabase.GraphQl.OpticalDataX;

[Node(IdField = nameof(Id))]
public sealed record OpticalData(
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
    OpticalComponentType? Type,
    OpticalComponentSubtype? Subtype,
    CoatedSide? CoatedSide,
    AppliedMethod AppliedMethod,
    IReadOnlyList<GetHttpsResource> Resources,
    GetHttpsResourceTree ResourceTree,
    IReadOnlyList<DataApproval> Approvals,
    // ResponseApproval Approval,
    IReadOnlyList<double> NearnormalHemisphericalVisibleTransmittances,
    IReadOnlyList<double> NearnormalHemisphericalVisibleReflectances,
    IReadOnlyList<double> NearnormalHemisphericalSolarTransmittances,
    IReadOnlyList<double> NearnormalHemisphericalSolarReflectances,
    IReadOnlyList<double> InfraredEmittances,
    IReadOnlyList<double> ColorRenderingIndices,
    IReadOnlyList<CielabColor> CielabColors
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
    public override DataKind Kind => DataKind.OPTICAL_DATA;

    [NodeResolver]
    public static Task<OpticalData?> GetAsync(
        string id,
        DataQueries dataQueries,
        ApplicationDbContext databaseContext,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return DataX.Data.FetchNodeAsync<OpticalData>(
            id,
            (database, uuid, locale) => dataQueries.GetOpticalDataAsync(
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