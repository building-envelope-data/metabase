using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class RemoveUserRoleError
      : UserErrorBase<RemoveUserRoleErrorCode>
    {
        public RemoveUserRoleError(
            RemoveUserRoleErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}