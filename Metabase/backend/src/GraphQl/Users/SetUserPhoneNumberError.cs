using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class SetUserPhoneNumberError
      : GraphQl.UserErrorBase<SetUserPhoneNumberErrorCode>
    {
        public SetUserPhoneNumberError(
            SetUserPhoneNumberErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}