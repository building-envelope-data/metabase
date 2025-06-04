using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationInstitutionConnection(
    OpenIdConnectApplication application
    )
: OpenIdConnectConnection<InstitutionOpenIdConnectApplication, InstitutionOpenIdConnectApplicationsByInstitutionIdDataLoader, InstitutionOpenIdConnectApplicationEdge>(
        application,
        x => new InstitutionOpenIdConnectApplicationEdge(x)
        )
{
}