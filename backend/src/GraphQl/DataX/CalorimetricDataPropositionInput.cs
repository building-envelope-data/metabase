using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
  public record CalorimetricDataPropositionInput(
      UuidPropositionInput? ComponentId,
      IReadOnlyList<CalorimetricDataPropositionInput>? And,
      FloatPropositionInput? GValue,
      CalorimetricDataPropositionInput? Not,
      IReadOnlyList<CalorimetricDataPropositionInput>? Or,
      FloatPropositionInput? UValue
      );
}
