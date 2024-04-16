using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Databases
{
    [SuppressMessage("Naming", "CA1707")]
    public enum UpdateDatabaseErrorCode
    {
        UNKNOWN,
        UNAUTHORIZED,
        UNKNOWN_DATABASE
    }
}