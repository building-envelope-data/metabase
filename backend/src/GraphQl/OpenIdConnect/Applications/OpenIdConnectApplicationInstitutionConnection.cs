using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationInstitutionConnection(
    OpenIdConnectApplication application
) : OpenIdConnectConnection<
    InstitutionOpenIdConnectApplication,
    OpenIdConnectApplicationInstitutionsByApplicationIdDataLoader,
    OpenIdConnectApplicationInstitutionEdge
>
(
    application,
    x => new OpenIdConnectApplicationInstitutionEdge(x)
)
{
}