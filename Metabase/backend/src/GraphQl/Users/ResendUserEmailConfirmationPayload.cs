using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ResendUserEmailConfirmationPayload
    {
        public IReadOnlyCollection<ResendUserEmailConfirmationError>? Errors { get; }

        public ResendUserEmailConfirmationPayload()
        {
        }
    }
}
