using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
  public record OpticalDataPropositionInput(
      UuidPropositionInput? ComponentId,
      IReadOnlyList<OpticalDataPropositionInput>? And,
      FloatPropositionInput? NearnormalHemisphericalVisibleTransmittance,
      OpticalDataPropositionInput? Not,
      IReadOnlyList<OpticalDataPropositionInput>? Or
      );
}
