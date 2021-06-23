using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class HygrothermalDataPropositionInput {
      public Guid ComponentId { get; set; }
      public List<HygrothermalDataPropositionInput> And { get; set; }
      public HygrothermalDataPropositionInput Not { get; set; }
      public List<HygrothermalDataPropositionInput> Or { get; set; }
    }
}
