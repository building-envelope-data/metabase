using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class DeleteOpenIdConnectAuthorizationPayload
{
    public DeleteOpenIdConnectAuthorizationPayload()
    {
    }

    public DeleteOpenIdConnectAuthorizationPayload(
        DeleteOpenIdConnectAuthorizationError error
    )
    {
        Errors = [error];
    }

    public IReadOnlyCollection<DeleteOpenIdConnectAuthorizationError>? Errors { get; }
}