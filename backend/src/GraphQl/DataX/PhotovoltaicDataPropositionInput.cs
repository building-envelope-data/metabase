using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
    public class PhotovoltaicDataPropositionInput {
      public Guid ComponentId { get; set; }
      public List<PhotovoltaicDataPropositionInput> And { get; set; }
      public PhotovoltaicDataPropositionInput Not { get; set; }
      public List<PhotovoltaicDataPropositionInput> Or { get; set; }
    }
}
