using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
  public record DataPropositionInput(
      UuidPropositionInput? ComponentId,
      List<DataPropositionInput>? And,
      FloatPropositionInput? GValue,
      FloatPropositionInput? NearnormalHemisphericalVisibleTransmittance,
      DataPropositionInput? Not,
      List<DataPropositionInput>? Or,
      FloatPropositionInput? UValue
      );
}
