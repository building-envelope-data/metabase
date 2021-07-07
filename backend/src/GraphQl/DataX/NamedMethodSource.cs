using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public sealed class NamedMethodSource {
      public string Name { get; set; }
      public CrossDatabaseDataReference Value { get; set; }
    }
}