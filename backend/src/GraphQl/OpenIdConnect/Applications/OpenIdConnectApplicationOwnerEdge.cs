using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

public sealed class OpenIdConnectApplicationOwnerEdge(
    OpenIdConnectApplication association
    )
        : Edge<Institution, InstitutionByIdDataLoader>(association.OwnerId)
{
}