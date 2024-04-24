using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.ComponentManufacturers;

[SuppressMessage("Naming", "CA1707")]
public enum ConfirmComponentManufacturerErrorCode
{
    UNKNOWN,
    UNKNOWN_COMPONENT,
    UNKNOWN_INSTITUTION,
    UNAUTHORIZED,
    UNKNOWN_MANUFACTURER
}