using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.ComponentAssemblies;

[SuppressMessage("Naming", "CA1707")]
public enum UpdateComponentAssemblyErrorCode
{
    UNKNOWN,
    UNKNOWN_ASSEMBLED_COMPONENT,
    UNKNOWN_PART_COMPONENT,
    UNKNOWN_ASSEMBLY,
    UNAUTHORIZED
}