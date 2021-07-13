using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
  public record HygrothermalDataPropositionInput(
      UuidPropositionInput? ComponentId,
      IReadOnlyList<HygrothermalDataPropositionInput>? And,
      HygrothermalDataPropositionInput? Not,
      IReadOnlyList<HygrothermalDataPropositionInput>? Or
      );
}
