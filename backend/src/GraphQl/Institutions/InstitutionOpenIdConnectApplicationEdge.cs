using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOpenIdConnectApplicationEdge(
    InstitutionOpenIdConnectApplication association
    ) : Edge<Institution, InstitutionByIdDataLoader>(association.ApplicationId)
{
}