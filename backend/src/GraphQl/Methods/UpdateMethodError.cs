using System.Collections.Generic;

namespace Metabase.GraphQl.Methods
{
    public sealed class UpdateMethodError
        : GraphQl.UserErrorBase<UpdateMethodErrorCode>
    {
        public UpdateMethodError(
            UpdateMethodErrorCode code,
            string message,
            IReadOnlyList<string> path
        )
            : base(code, message, path)
        {
        }
    }
}