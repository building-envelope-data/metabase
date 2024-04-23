using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class RequestUserPasswordResetPayload
{
    public IReadOnlyCollection<RequestUserPasswordResetError>? Errors { get; }
}