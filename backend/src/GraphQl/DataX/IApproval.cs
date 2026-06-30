using System;
using System.Text.Json;
using HotChocolate;
using HotChocolate.Types;
using Metabase.GraphQl.Scalars;
using NodaTime;

namespace Metabase.GraphQl.DataX;

[InterfaceType("Approval")]
public interface IApproval
{
    OffsetDateTime Timestamp { get; }
    string Signature { get; }
    string KeyFingerprint { get; }
    [GraphQLType<NonNullType<GraphQlQueryType>>] string Query { get; }
    JsonElement Variables { get; }
    string Message { get; }
    Guid ApproverId { get; }
}