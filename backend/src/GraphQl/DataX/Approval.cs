using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public interface Approval {
      DateTime Timestamp { get; set; }
      string Signature { get; set; }
      string KeyFingerprint { get; set; }
      string Query { get; set; }
      string Response { get; set; }
    }
}
