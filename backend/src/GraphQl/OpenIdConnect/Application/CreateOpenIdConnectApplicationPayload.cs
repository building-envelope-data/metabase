using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Application;

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
}