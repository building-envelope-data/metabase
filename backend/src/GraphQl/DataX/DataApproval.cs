using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.DataX;

public sealed class DataApproval(
    DateTime timestamp,
    string signature,
    string keyFingerprint,
    string query,
    JsonElement variables,
    string response,
    Guid approverId
    )
        : IApproval
{
    public Guid ApproverId { get; } = approverId;
    public DateTime Timestamp { get; } = timestamp;
    public string Signature { get; } = signature;
    public string KeyFingerprint { get; } = keyFingerprint;
    public string Query { get; } = query;
    public JsonElement Variables { get; } = variables;
    public string Response { get; } = response;

    public Task<Institution?> GetApproverAsync(
        InstitutionByIdDataLoader institutionById,
        CancellationToken cancellationToken
    )
    {
        return institutionById.LoadAsync(
            ApproverId,
            cancellationToken
        );
    }
}