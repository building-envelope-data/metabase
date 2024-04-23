using System.Collections.Generic;
using Metabase.GraphQl.Methods;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.InstitutionMethodDevelopers;

public sealed class AddInstitutionMethodDeveloperPayload
{
    public InstitutionDevelopedMethodEdge? DevelopedMethodEdge { get; }
    public InstitutionMethodDeveloperEdge? MethodDeveloperEdge { get; }
    public IReadOnlyCollection<AddInstitutionMethodDeveloperError>? Errors { get; }

    public AddInstitutionMethodDeveloperPayload(
        Data.InstitutionMethodDeveloper institutionMethodDeveloper
    )
    {
        DevelopedMethodEdge = new InstitutionDevelopedMethodEdge(institutionMethodDeveloper);
        MethodDeveloperEdge = new InstitutionMethodDeveloperEdge(institutionMethodDeveloper);
    }

    public AddInstitutionMethodDeveloperPayload(
        IReadOnlyCollection<AddInstitutionMethodDeveloperError> errors
    )
    {
        Errors = errors;
    }

    public AddInstitutionMethodDeveloperPayload(
        AddInstitutionMethodDeveloperError error
    )
        : this(new[] { error })
    {
    }
}