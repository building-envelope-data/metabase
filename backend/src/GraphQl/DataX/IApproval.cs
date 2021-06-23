using System;
using HotChocolate.Types;

namespace Metabase.GraphQl.DataX
{
    [InterfaceType("Approval")]
    public interface IApproval {
      DateTime Timestamp { get; set; }
      string Signature { get; set; }
      string KeyFingerprint { get; set; }
      string Query { get; set; }
      string Response { get; set; }
    }
}
