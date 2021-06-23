using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
  public record CalorimetricDataPropositionInput(
      Guid? ComponentId,
      List<CalorimetricDataPropositionInput>? And,
      FloatPropositionInput? GValue,
      CalorimetricDataPropositionInput? Not,
      List<CalorimetricDataPropositionInput>? Or,
      FloatPropositionInput? UValue
      );
}
