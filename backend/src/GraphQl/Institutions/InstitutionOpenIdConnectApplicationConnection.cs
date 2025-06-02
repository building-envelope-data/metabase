using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionApplicationConnection(
    Institution institution
    )
        : Connection<Institution, InstitutionOpenIdConnectApplication,
        InstitutionApplicationsByInstitutionIdDataLoader, InstitutionApplicationEdge>(
        institution,
        x => new InstitutionApplicationEdge(x)
        )
{
}