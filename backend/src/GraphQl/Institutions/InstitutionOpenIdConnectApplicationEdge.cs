using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionApplicationEdge(
    InstitutionOpenIdConnectApplication association
    ) : Edge<Institution, InstitutionByIdDataLoader>(association.ApplicationId)
{
}