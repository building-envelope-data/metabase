using Metabase.Data;
using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionDevelopedMethodEdge
    : Edge<Method, MethodByIdDataLoader>
{
    public InstitutionDevelopedMethodEdge(
        InstitutionMethodDeveloper association
    )
        : base(association.MethodId)
    {
    }
}