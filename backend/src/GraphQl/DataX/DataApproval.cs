using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Institutions;
using NodaTime;

namespace Metabase.GraphQl.DataX;

public sealed class DataApproval(
    OffsetDateTime timestamp,
    string signature,
    string keyFingerprint,
    string query,
    JsonElement variables,
    string message,
    Guid approverId,
    IReference statement
    )
        : IApproval
{
    public Guid ApproverId { get; } = approverId;
    public OffsetDateTime Timestamp { get; } = timestamp;
    public string Signature { get; } = signature;
    public string KeyFingerprint { get; } = keyFingerprint;
    public string Query { get; } = query;
    public JsonElement Variables { get; } = variables;
    public string Message { get; } = message;
    public IReference Statement { get; private set; } = statement;

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