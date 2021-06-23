using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class NamedMethodArgument {
      public string Name { get; set; }
      public object Value { get; set; }
    }
}