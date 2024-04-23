using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Methods;

[SuppressMessage("Naming", "CA1707")]
public enum CreateMethodErrorCode
{
    UNKNOWN,
    TWO_REFERENCES,
    UNKNOWN_MANAGER,
    UNKNOWN_INSTITUTION_DEVELOPERS,
    UNKNOWN_USER_DEVELOPERS,
    UNAUTHORIZED
}