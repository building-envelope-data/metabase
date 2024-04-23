using System;

namespace Metabase.GraphQl.DataX
{
    public sealed class ResponseApproval
        : IApproval
    {
        public ResponseApproval(
            DateTime timestamp,
            string signature,
            string keyFingerprint,
            string query,
            string response
        )
        {
            Timestamp = timestamp;
            Signature = signature;
            KeyFingerprint = keyFingerprint;
            Query = query;
            Response = response;
        }

        public DateTime Timestamp { get; }
        public string Signature { get; }
        public string KeyFingerprint { get; }
        public string Query { get; }
        public string Response { get; }
    }
}