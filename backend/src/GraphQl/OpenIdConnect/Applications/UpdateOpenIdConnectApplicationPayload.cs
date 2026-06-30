using System.Collections.Generic;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

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

    public UpdateOpenIdConnectApplicationPayload(
        IReadOnlyList<UpdateOpenIdConnectApplicationError> errors
    )
        : base(errors)
    {
    }
}