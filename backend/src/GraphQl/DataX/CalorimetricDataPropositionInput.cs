using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class CalorimetricDataPropositionInput {
      public Guid ComponentId { get; set; }
      public List<CalorimetricDataPropositionInput> And { get; set; }
      public FloatPropositionInput GValue { get; set; }
      public CalorimetricDataPropositionInput Not { get; set; }
      public List<CalorimetricDataPropositionInput> Or { get; set; }
      public FloatPropositionInput UValue { get; set; }
    }
}
