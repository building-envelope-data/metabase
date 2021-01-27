using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class ConfirmUserEmailChangeError
      : GraphQl.UserErrorBase<ConfirmUserEmailChangeErrorCode>
    {
        public ConfirmUserEmailChangeError(
            ConfirmUserEmailChangeErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}