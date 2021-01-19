using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class PersonalUserDataError
      : UserErrorBase<PersonalUserDataErrorCode>
    {
        public PersonalUserDataError(
            PersonalUserDataErrorCode code,
            string message,
            IReadOnlyList<string> path
            )
          : base(code, message, path)
        {
        }
    }
}
