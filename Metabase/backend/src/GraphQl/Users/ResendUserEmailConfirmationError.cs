using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ResendUserEmailConfirmationError
    : GraphQl.UserErrorBase<ResendUserEmailConfirmationErrorCode>
  {
    public ResendUserEmailConfirmationError(
        ResendUserEmailConfirmationErrorCode code,
        string message,
        IReadOnlyList<string> path
        )
      : base(code, message, path)
    {
    }
  }
}
