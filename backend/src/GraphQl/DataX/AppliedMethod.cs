using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class AppliedMethod {
      public Guid MethodId { get; set; }
      public List<NamedMethodArgument> Arguments { get; set; }
      public List<NamedMethodSource> Sources { get; set; }
    }
}