using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.OpenIdConnect.Application;

public sealed class OpenIdConnectApplicationInstitutionEdge(
    InstitutionOpenIdConnectApplication association
) : Edge<Institution, InstitutionByIdDataLoader>
(
    association.InstitutionId
)
{
}