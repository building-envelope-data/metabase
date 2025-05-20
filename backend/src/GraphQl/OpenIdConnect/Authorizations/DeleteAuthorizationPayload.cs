using Metabase.GraphQl.OpenIdConnect.Application;
using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class DeleteAuthorizationPayload
{
    public DeleteAuthorizationPayload()
    {
    }

    public DeleteAuthorizationPayload(
        DeleteAuthorizationError error
    )
    {
        Errors = [error];
    }

    public IReadOnlyCollection<DeleteAuthorizationError>? Errors { get; }
}