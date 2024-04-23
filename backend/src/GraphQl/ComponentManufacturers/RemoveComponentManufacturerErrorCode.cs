using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.ComponentManufacturers;

[SuppressMessage("Naming", "CA1707")]
public enum RemoveComponentManufacturerErrorCode
{
    UNKNOWN,
    UNKNOWN_COMPONENT,
    UNKNOWN_INSTITUTION,
    UNAUTHORIZED,
    UNKNOWN_MANUFACTURER,
    LAST_MANUFACTURER
}