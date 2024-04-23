using Metabase.Data;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.DataFormats;

public sealed class DataFormatManagerEdge
    : Edge<Institution, InstitutionByIdDataLoader>
{
    public DataFormatManagerEdge(
        DataFormat association
    )
        : base(association.ManagerId)
    {
    }
}