using System;
using System.Text.Json;

namespace Metabase.GraphQl.DataX;

public sealed record ResponseApproval(
    DateTime Timestamp,
    string Signature,
    string KeyFingerprint,
    string Query,
    JsonElement Variables,
    string Message,
    Guid ApproverId
) : IApproval;