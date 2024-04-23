using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class LogoutUserPayload
{
    public User? User { get; }
    public IReadOnlyCollection<LogoutUserError>? Errors { get; }
}