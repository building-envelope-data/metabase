using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class SetUserPasswordError
    : GraphQl.UserErrorBase<SetUserPasswordErrorCode>
  {
    public SetUserPasswordError(
        SetUserPasswordErrorCode code,
        string message,
        IReadOnlyList<string> path
        )
      : base(code, message, path)
    {
    }
  }
}
