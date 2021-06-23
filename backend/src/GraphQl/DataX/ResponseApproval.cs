using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class ResponseApproval : Approval {
      public DateTime Timestamp { get; set; }
      public string Signature { get; set; }
      public string KeyFingerprint { get; set; }
      public string Query { get; set; }
      public string Response { get; set; }
    }
}
