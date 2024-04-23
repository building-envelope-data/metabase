using System;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManagerEdge
    : Edge<Data.Institution, InstitutionByIdDataLoader>
{
    public InstitutionManagerEdge(
        Data.Institution association
    )
        : base(association.ManagerId ?? Guid.Empty)
    {
    }
}