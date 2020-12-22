using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ChangeUserPasswordError
    : GraphQl.UserErrorBase<ChangeUserPasswordErrorCode>
  {
    public ChangeUserPasswordError(
        ChangeUserPasswordErrorCode code,
        string message,
        IReadOnlyList<string> path
        )
      : base(code, message, path)
    {
    }
  }
}
