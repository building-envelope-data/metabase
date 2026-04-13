using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.DataX;

[SuppressMessage("Naming", "CA1707")]
public enum CoatedSide
{
    PRIME,
    NON_PRIME,
    BOTH,
    NEITHER,
    UNKNOWN,
    NOT_APPLICABLE
}