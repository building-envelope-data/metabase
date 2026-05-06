using System;
using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagerEdge(
    Institution association
    )
        : Edge<Institution, IInstitutionByIdDataLoader>(association.ManagerId ?? Guid.Empty)
{
}