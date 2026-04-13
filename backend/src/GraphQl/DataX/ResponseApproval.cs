using System;
using System.Text.Json;
using NodaTime;

namespace Metabase.GraphQl.DataX;

public sealed record ResponseApproval(
    OffsetDateTime Timestamp,
    string Signature,
    string KeyFingerprint,
    string Query,
    JsonElement Variables,
    string Message,
    Guid ApproverId
) : IApproval;