using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
  public record HygrothermalDataPropositionInput(
      Guid? ComponentId,
      List<HygrothermalDataPropositionInput>? And,
      HygrothermalDataPropositionInput? Not,
      List<HygrothermalDataPropositionInput>? Or
      );
}
