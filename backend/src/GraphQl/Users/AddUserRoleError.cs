using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class AddUserRoleError
      : UserErrorBase<AddUserRoleErrorCode>
    {
        public AddUserRoleError(
            AddUserRoleErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}