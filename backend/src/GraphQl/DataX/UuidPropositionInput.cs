using System;

namespace Metabase.GraphQl.DataX;

public sealed record UuidPropositionInput(
    Guid? EqualTo
)
{
    public static readonly UuidPropositionInput Empty = new(EqualTo: null);
};