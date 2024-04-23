using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentGeneralizations
{
    public sealed class RemoveComponentGeneralizationError
        : UserErrorBase<RemoveComponentGeneralizationErrorCode>
    {
        public RemoveComponentGeneralizationError(
            RemoveComponentGeneralizationErrorCode code,
            string message,
            IReadOnlyList<string> path
        )
            : base(code, message, path)
        {
        }
    }
}