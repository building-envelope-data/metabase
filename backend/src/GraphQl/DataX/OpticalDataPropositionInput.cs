using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class OpticalDataPropositionInput {
      public Guid ComponentId { get; set; }
      public List<OpticalDataPropositionInput> And { get; set; }
      public FloatPropositionInput NearnormalHemisphericalVisibleTransmittance { get; set; }
      public OpticalDataPropositionInput Not { get; set; }
      public List<OpticalDataPropositionInput> Or { get; set; }
    }
}
