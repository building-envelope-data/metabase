using System;
using System.Collections.Generic;

namespace Metabase.GraphQl.DataX
{
  public record PhotovoltaicDataPropositionInput(
      Guid? ComponentId,
      List<PhotovoltaicDataPropositionInput>? And,
      PhotovoltaicDataPropositionInput? Not,
      List<PhotovoltaicDataPropositionInput>? Or
      );
}
