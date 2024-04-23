using Metabase.GraphQl.Methods;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionDevelopedMethodEdge
    : Edge<Data.Method, MethodByIdDataLoader>
{
    public InstitutionDevelopedMethodEdge(
        Data.InstitutionMethodDeveloper association
    )
        : base(association.MethodId)
    {
    }
}