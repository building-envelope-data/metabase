using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
  public record OpticalDataPropositionInput(
      Guid? ComponentId,
      List<OpticalDataPropositionInput>? And,
      FloatPropositionInput? NearnormalHemisphericalVisibleTransmittance,
      OpticalDataPropositionInput? Not,
      List<OpticalDataPropositionInput>? Or
      );
}
