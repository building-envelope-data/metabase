using HotChocolate.Types;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.DataFormats
{
    public sealed class DataFormatManagerEdge
        : Edge<Data.Institution, InstitutionByIdDataLoader>
    {
        public DataFormatManagerEdge(
            Data.DataFormat association
        )
            : base(association.ManagerId)
        {
        }
    }
}