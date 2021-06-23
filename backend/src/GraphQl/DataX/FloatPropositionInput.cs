using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class FloatPropositionInput {
      public double? EqualTo { get; set; }
      public double? GreaterThanOrEqualTo { get; set; }
      public ClosedIntervalInput InClosedInterval { get; set; }
      public double? LessThanOrEqualTo { get; set; }
    }
}
