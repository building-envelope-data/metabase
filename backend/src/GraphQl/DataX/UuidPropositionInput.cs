using System;

namespace Metabase.GraphQl.DataX;

public sealed record UuidPropositionInput(
    Guid? EqualTo
);