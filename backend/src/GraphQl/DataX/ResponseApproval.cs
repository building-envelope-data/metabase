using System;
using System.Text.Json;
using HotChocolate;
using HotChocolate.Types;
using Metabase.GraphQl.Scalars;
using NodaTime;

namespace Metabase.GraphQl.DataX;

public sealed record ResponseApproval(
    OffsetDateTime Timestamp,
    string Signature,
    string KeyFingerprint,
    [property: GraphQLType<NonNullType<GraphQlQueryType>>] string Query,
    JsonElement Variables,
    string Message,
    Guid ApproverId
) : IApproval;