using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.UserMethodDevelopers
{
    [SuppressMessage("Naming", "CA1707")]
    public enum AddUserMethodDeveloperErrorCode
    {
        UNKNOWN,
        UNKNOWN_METHOD,
        UNKNOWN_USER,
        UNAUTHORIZED,
        DUPLICATE
    }
}