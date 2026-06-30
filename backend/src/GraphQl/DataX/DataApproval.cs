using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Institutions;
using Metabase.GraphQl.Scalars;
using NodaTime;

namespace Metabase.GraphQl.DataX;

public sealed record DataApproval(
    OffsetDateTime Timestamp,
    string Signature,
    string KeyFingerprint,
    [property: GraphQLType<NonNullType<GraphQlQueryType>>] string Query,
    JsonElement Variables,
    string Message,
    Guid ApproverId,
    IReference Statement
)
: IApproval
{
    public Task<Institution?> GetApproverAsync(
        IInstitutionByIdDataLoader institutionById,
        CancellationToken cancellationToken
    )
    {
        return institutionById.LoadAsync(
            ApproverId,
            cancellationToken
        );
    }
}