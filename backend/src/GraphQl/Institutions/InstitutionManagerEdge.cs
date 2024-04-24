using System;
using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagerEdge
    : Edge<Institution, InstitutionByIdDataLoader>
{
    public InstitutionManagerEdge(
        Institution association
    )
        : base(association.ManagerId ?? Guid.Empty)
    {
    }
}