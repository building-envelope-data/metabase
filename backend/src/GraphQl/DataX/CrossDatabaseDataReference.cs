using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class CrossDatabaseDataReference {
      public Guid DataId { get; set; }
      public DateTime DataTimestamp { get; set; }
      public DataKind DataKind { get; set; }
      public Guid DatabaseId { get; set; }
    }
}
