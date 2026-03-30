using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Metabase.Data;
using Metabase.Extensions;
using Metabase.GraphQl.Components;
using Metabase.GraphQl.Databases;
using Metabase.GraphQl.Institutions;
using Metabase.GraphQl.Scalars;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Metabase.GraphQl.DataX;

public abstract partial record Data(
    [property: GraphQLIgnore] string DataId,
    Guid Uuid,
    OffsetDateTime Timestamp,
    [property: GraphQLType<NonNullType<LocaleType>>] string Locale,
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
// ResponseApproval approval
)
: IData
{
    [GeneratedRegex("^(?<databaseId>[0-9a-f]{8}-(?:[0-9a-f]{4}-){3}[0-9a-f]{12}):(?<uuid>[0-9a-f]{8}-(?:[0-9a-f]{4}-){3}[0-9a-f]{12}):(?<locale>[^:]*):(?<dataId>.+)$", RegexOptions.IgnoreCase)]
    private static partial Regex IdRegex();

    public static async Task<TData?> FetchNodeAsync<TData>(
        [ID] string id,
        Func<Database, Guid, string?, Task<TData?>> getDataAsync,
        ApplicationDbContext databaseContext,
        CancellationToken cancellationToken
    )
        where TData : class, IData
    {
        var match = IdRegex().Match(id);
        if (match is null || !match.Success)
        {
            return null;
        }
        var databaseId = new Guid(match.Groups["databaseId"].Value);
        var uuid = new Guid(match.Groups["uuid"].Value);
        var locale = match.Groups["locale"].Value.NullIfWhitespace();
        // TODO use only the following dataId instead of uuid and locale and use the product-data database's query `node(id: ID)` instead of `*Data(id: Uuid)`: var dataId = match.Groups["dataId"].Value;
        var database = await databaseContext.Databases.AsNoTracking()
            .Where(_ => _.Id == databaseId)
            .SingleOrDefaultAsync(cancellationToken);
        if (database is null)
        {
            return null;
        }
        return await getDataAsync(database, uuid, locale);
    }

    [ID]
    public string Id => Convert.ToBase64String(
        Encoding.UTF8.GetBytes($"{DatabaseId}:{Uuid}:{Locale ?? ""}:{DataId}")
    );

    public abstract DataKind Kind { get; }

    public Task<Database?> GetDatabaseAsync(
        IDatabaseByIdDataLoader databaseById,
        CancellationToken cancellationToken
    )
    {
        return databaseById.LoadAsync(
            DatabaseId,
            cancellationToken
            );
    }

    public Task<Component?> GetComponentAsync(
        IComponentByIdDataLoader componentById,
        CancellationToken cancellationToken
    )
    {
        return componentById.LoadAsync(
            ComponentId,
            cancellationToken
        );
    }

    public Task<Institution?> GetCreatorAsync(
        IInstitutionByIdDataLoader institutionById,
        CancellationToken cancellationToken
    )
    {
        return institutionById.LoadAsync(
            CreatorId,
            cancellationToken
        );
    }
}