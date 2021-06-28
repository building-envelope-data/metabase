using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
  public record HygrothermalDataPropositionInput(
      UuidPropositionInput? ComponentId,
      List<HygrothermalDataPropositionInput>? And,
      HygrothermalDataPropositionInput? Not,
      List<HygrothermalDataPropositionInput>? Or
      );
}
