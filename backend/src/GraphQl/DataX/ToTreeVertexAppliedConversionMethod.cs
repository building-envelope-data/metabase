using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class ToTreeVertexAppliedConversionMethod {
      public Guid MethodId { get; set; }
      public List<NamedMethodArgument> Arguments { get; set; }
      public string SourceName { get; set; }
    }
}
