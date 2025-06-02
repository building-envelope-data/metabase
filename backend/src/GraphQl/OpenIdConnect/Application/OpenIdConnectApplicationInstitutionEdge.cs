using Metabase.Data;
using Metabase.Data.OpenIdConnect;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationInstitutionEdge(
    InstitutionOpenIdConnectApplication association
    ) : Edge<OpenIdConnectApplication, OpenIdConnectApplicationByIdDataLoader>(association.InstitutionId)
{
}