using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class LogoutUserPayload
    {
        public Data.User? User { get; }
        public IReadOnlyCollection<LogoutUserError>? Errors { get; }
    }
}