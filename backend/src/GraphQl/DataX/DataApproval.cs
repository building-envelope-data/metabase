using System;

namespace Metabase.GraphQl.DataX
{
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

      public DateTime Timestamp { get; }
      public string Signature { get; }
      public string KeyFingerprint { get; }
      public string Query { get; }
      public string Response { get; }
      public Guid ApproverId { get; }
    }
}
