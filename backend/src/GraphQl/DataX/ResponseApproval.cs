using System;
using System.Text.Json;

namespace Metabase.GraphQl.DataX;

public sealed class ResponseApproval(
    DateTime timestamp,
    string signature,
    string keyFingerprint,
    string query,
    JsonElement variables,
    string message
    )
        : IApproval
{
    public DateTime Timestamp { get; } = timestamp;
    public string Signature { get; } = signature;
    public string KeyFingerprint { get; } = keyFingerprint;
    public string Query { get; } = query;
    public JsonElement Variables { get; } = variables;
    public string Message { get; } = message;
}