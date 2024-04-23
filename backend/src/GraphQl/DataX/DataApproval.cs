using System;
using System.Threading;
using System.Threading.Tasks;
using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.DataX;

public sealed class DataApproval
    : IApproval
{
    public DataApproval(
        DateTime timestamp,
        string signature,
        string keyFingerprint,
        string query,
        string response,
        Guid approverId
    )
    {
        Timestamp = timestamp;
        Signature = signature;
        KeyFingerprint = keyFingerprint;
        Query = query;
        Response = response;
        ApproverId = approverId;
    }

    public Guid ApproverId { get; }
    public DateTime Timestamp { get; }
    public string Signature { get; }
    public string KeyFingerprint { get; }
    public string Query { get; }
    public string Response { get; }

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