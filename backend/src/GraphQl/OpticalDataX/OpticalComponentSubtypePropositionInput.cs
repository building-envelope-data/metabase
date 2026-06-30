using System.Collections.Generic;

namespace Metabase.GraphQl.OpticalDataX;

public sealed record OpticalComponentSubtypePropositionInput(
  OpticalComponentSubtype? EqualTo,
  OpticalComponentSubtype? NotEqualTo,
  IReadOnlyList<OpticalComponentSubtype?>? In,
  IReadOnlyList<OpticalComponentSubtype?>? NotIn
);