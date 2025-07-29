using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Components;

[SuppressMessage("Naming", "CA1707")]
public enum UpdateComponentErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_COMPONENT,
    UNKNOWN_MANUFACTURER,
    AMBIGUOUS_REFERENCE
}