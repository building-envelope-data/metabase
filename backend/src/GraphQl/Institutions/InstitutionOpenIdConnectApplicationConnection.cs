using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionOpenIdConnectApplicationConnection(
    Institution institution
    )
        : Connection<Institution, InstitutionOpenIdConnectApplication,
        InstitutionOpenIdConnectApplicationsByInstitutionIdDataLoader, InstitutionOpenIdConnectApplicationEdge>(
        institution,
        x => new InstitutionOpenIdConnectApplicationEdge(x)
        )
{
}