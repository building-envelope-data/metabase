using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class AppliedMethod {
      public AppliedMethod(
        Guid methodId
      )
      {
        MethodId = methodId;
      }

      public Guid MethodId { get; }
      // public IReadOnlyList<NamedMethodArgument> Arguments { get; }
      // public IReadOnlyList<NamedMethodSource> Sources { get; }
    }
}