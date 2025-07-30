using System.Collections.Generic;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class DeleteOpenIdConnectApplicationPayload
{
    public DeleteOpenIdConnectApplicationPayload()
    {
    }

    public DeleteOpenIdConnectApplicationPayload(
        DeleteOpenIdConnectApplicationError error
    )
    {
        Errors = [error];
    }

    public IReadOnlyCollection<DeleteOpenIdConnectApplicationError>? Errors { get; }
}