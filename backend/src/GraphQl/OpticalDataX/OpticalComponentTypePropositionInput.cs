using System.Collections.Generic;

namespace Metabase.GraphQl.OpticalDataX;

public sealed record OpticalComponentTypePropositionInput(
  OpticalComponentType? EqualTo,
  OpticalComponentType? NotEqualTo,
  IReadOnlyList<OpticalComponentType>? In,
  IReadOnlyList<OpticalComponentType>? NotIn
);