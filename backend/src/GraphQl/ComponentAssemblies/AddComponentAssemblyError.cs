using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentAssemblies;

public sealed class AddComponentAssemblyError
    : UserErrorBase<AddComponentAssemblyErrorCode>
{
    public AddComponentAssemblyError(
        AddComponentAssemblyErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}