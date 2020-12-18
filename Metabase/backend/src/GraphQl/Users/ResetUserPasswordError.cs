using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ResetUserPasswordError
    : GraphQl.UserErrorBase<ResetUserPasswordErrorCode>
  {
    public ResetUserPasswordError(
        ResetUserPasswordErrorCode code,
        string message,
        IReadOnlyList<string> path
        )
      : base(code, message, path)
    {
    }
  }
}
