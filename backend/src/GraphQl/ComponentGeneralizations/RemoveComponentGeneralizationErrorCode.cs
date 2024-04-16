using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.ComponentGeneralizations
{
    [SuppressMessage("Naming", "CA1707")]
    public enum RemoveComponentGeneralizationErrorCode
    {
        UNKNOWN,
        UNKNOWN_GENERAL_COMPONENT,
        UNKNOWN_CONCRETE_COMPONENT,
        UNKNOWN_GENERALIZATION,
        UNAUTHORIZED
    }
}