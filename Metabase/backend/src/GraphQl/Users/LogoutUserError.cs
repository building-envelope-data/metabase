using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class LogoutUserError
      : GraphQl.UserErrorBase<LogoutUserErrorCode>
    {
        public LogoutUserError(
            LogoutUserErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}