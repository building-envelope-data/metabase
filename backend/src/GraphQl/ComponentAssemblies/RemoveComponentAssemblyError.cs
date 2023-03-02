using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentAssemblies
{
    public sealed class RemoveComponentAssemblyError
      : UserErrorBase<RemoveComponentAssemblyErrorCode>
    {
        public RemoveComponentAssemblyError(
            RemoveComponentAssemblyErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}