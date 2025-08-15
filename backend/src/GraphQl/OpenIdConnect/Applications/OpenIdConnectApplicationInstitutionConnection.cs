using GreenDonut.Data;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationInstitutionConnection(
    OpenIdConnectApplication application,
    QueryContext<InstitutionOpenIdConnectApplication> queryContext
) : Connection<
    OpenIdConnectApplication,
    InstitutionOpenIdConnectApplication,
    OpenIdConnectApplicationInstitutionsByApplicationIdDataLoader,
    OpenIdConnectApplicationInstitutionEdge
>
(
    application,
    x => new OpenIdConnectApplicationInstitutionEdge(x),
    queryContext
)
{
}