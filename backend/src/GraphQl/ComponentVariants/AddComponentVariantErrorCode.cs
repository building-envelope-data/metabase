using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.ComponentVariants;

[SuppressMessage("Naming", "CA1707")]
public enum AddComponentVariantErrorCode
{
    UNKNOWN,
    UNKNOWN_ONE_COMPONENT,
    UNKNOWN_OTHER_COMPONENT,
    DUPLICATE,
    UNAUTHORIZED
}