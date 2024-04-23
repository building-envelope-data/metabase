using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Databases;

[SuppressMessage("Naming", "CA1707")]
public enum VerifyDatabaseErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_DATABASE,
    WRONG_VERIFICATION_CODE,
    REQUEST_FAILED,
    DESERIALIZATION_FAILED
}