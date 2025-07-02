using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Components;

[SuppressMessage("Naming", "CA1707")]
public enum SetComponentExtrasErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_COMPONENT
}