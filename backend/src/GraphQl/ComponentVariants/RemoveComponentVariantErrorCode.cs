using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.ComponentVariants;

[SuppressMessage("Naming", "CA1707")]
public enum RemoveComponentVariantErrorCode
{
    UNKNOWN,
    UNKNOWN_ONE_COMPONENT,
    UNKNOWN_OTHER_COMPONENT,
    UNKNOWN_VARIANT,
    UNAUTHORIZED
}