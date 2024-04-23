using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed class UpdateComponentAssemblyError
        : UserErrorBase<UpdateComponentAssemblyErrorCode>
    {
        public UpdateComponentAssemblyError(
            UpdateComponentAssemblyErrorCode code,
            string message,
            IReadOnlyList<string> path
        )
            : base(code, message, path)
        {
        }
    }
}