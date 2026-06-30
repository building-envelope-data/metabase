using System.Collections.Generic;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Authorizations;

public sealed class DeleteOpenIdConnectAuthorizationPayload
{
    public DeleteOpenIdConnectAuthorizationPayload(OpenIdConnectApplication? application)
    {
        Application = application;
    }

    public DeleteOpenIdConnectAuthorizationPayload(
        DeleteOpenIdConnectAuthorizationError error
    )
    {
        Errors = [error];
    }

    public OpenIdConnectApplication? Application { get; }
    public IReadOnlyCollection<DeleteOpenIdConnectAuthorizationError>? Errors { get; }
}