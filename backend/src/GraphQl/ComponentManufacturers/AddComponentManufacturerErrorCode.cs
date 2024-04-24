using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.ComponentManufacturers;

[SuppressMessage("Naming", "CA1707")]
public enum AddComponentManufacturerErrorCode
{
    UNKNOWN,
    UNKNOWN_COMPONENT,
    UNKNOWN_INSTITUTION,
    UNAUTHORIZED,
    DUPLICATE
}