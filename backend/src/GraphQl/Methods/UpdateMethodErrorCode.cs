using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Methods;

[SuppressMessage("Naming", "CA1707")]
public enum UpdateMethodErrorCode
{
    UNKNOWN,
    TWO_REFERENCES,
    UNAUTHORIZED,
    UNKNOWN_METHOD
}