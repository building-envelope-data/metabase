using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class ResetUserPasswordPayload
    {
        public IReadOnlyCollection<ResetUserPasswordError>? Errors { get; }

        public ResetUserPasswordPayload()
        {
        }

        public ResetUserPasswordPayload(
            IReadOnlyCollection<ResetUserPasswordError> errors
            )
        {
            Errors = errors;
        }

        public ResetUserPasswordPayload(
            ResetUserPasswordError error
            )
          : this(new[] { error })
        {
        }
    }
}