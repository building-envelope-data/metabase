using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class DataPropositionInput {
      public UuidPropositionInput ComponentId { get; set; }
      public List<DataPropositionInput> And { get; set; }
      public FloatPropositionInput GValue { get; set; }
      public FloatPropositionInput NearnormalHemisphericalVisibleTransmittance { get; set; }
      public DataPropositionInput Not { get; set; }
      public List<DataPropositionInput> Or { get; set; }
      public FloatPropositionInput UValue { get; set; }
    }
}
