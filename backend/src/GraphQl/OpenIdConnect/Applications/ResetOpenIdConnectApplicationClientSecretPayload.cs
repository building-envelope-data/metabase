using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class ResetOpenIdConnectApplicationClientSecretPayload
    : OpenIdConnectApplicationPayload<ResetOpenIdConnectApplicationClientSecretError>
{
    public string? ClientSecret { get; }

    public ResetOpenIdConnectApplicationClientSecretPayload(
        OpenIdConnectApplication application,
        string clientSecret
    )
        : base(application)
    {
        ClientSecret = clientSecret;
    }

    public ResetOpenIdConnectApplicationClientSecretPayload(
        ResetOpenIdConnectApplicationClientSecretError error
    )
        : base(error)
    {
    }
}