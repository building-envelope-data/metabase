using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.UserMethodDevelopers;

[SuppressMessage("Naming", "CA1707")]
public enum RemoveUserMethodDeveloperErrorCode
{
    UNKNOWN,
    UNKNOWN_METHOD,
    UNKNOWN_USER,
    UNAUTHORIZED,
    UNKNOWN_DEVELOPER
}