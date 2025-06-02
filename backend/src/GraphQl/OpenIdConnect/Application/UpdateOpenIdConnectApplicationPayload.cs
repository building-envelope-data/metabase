using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class UpdateOpenIdConnectApplicationPayload
    : OpenIdConnectApplicationPayload<UpdateOpenIdConnectApplicationError>
{
    public UpdateOpenIdConnectApplicationPayload(
        OpenIdConnectApplication application
    )
        : base(application)
    {
    }

    public UpdateOpenIdConnectApplicationPayload(
        UpdateOpenIdConnectApplicationError error
    )
        : base(error)
    {
    }
}