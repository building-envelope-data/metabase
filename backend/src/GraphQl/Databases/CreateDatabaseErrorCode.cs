using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Databases;

[SuppressMessage("Naming", "CA1707")]
public enum CreateDatabaseErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_OPERATOR
}