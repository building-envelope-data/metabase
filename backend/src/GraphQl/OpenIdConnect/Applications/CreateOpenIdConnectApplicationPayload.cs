using System.Collections.Generic;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class CreateOpenIdConnectApplicationPayload
    : OpenIdConnectApplicationPayload<CreateOpenIdConnectApplicationError>
{
    public string? ClientSecret { get; }

    public CreateOpenIdConnectApplicationPayload(
        OpenIdConnectApplication application,
        string clientSecret
    )
        : base(application)
    {
        ClientSecret = clientSecret;
    }

    public CreateOpenIdConnectApplicationPayload(
        CreateOpenIdConnectApplicationError error
    )
        : base(error)
    {
    }

    public CreateOpenIdConnectApplicationPayload(
        IReadOnlyCollection<CreateOpenIdConnectApplicationError> errors
    )
        : base(errors)
    {
    }
}