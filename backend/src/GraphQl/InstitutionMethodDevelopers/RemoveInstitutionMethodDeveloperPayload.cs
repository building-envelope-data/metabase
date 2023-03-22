using System.Collections.Generic;
using Metabase.GraphQl.Methods;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public sealed class RemoveInstitutionMethodDeveloperPayload
    {
        public InstitutionDevelopedMethodEdge? DevelopedMethodEdge { get; }
        public InstitutionMethodDeveloperEdge? MethodDeveloperEdge { get; }
        public IReadOnlyCollection<RemoveInstitutionMethodDeveloperError>? Errors { get; }

        public RemoveInstitutionMethodDeveloperPayload(
            Data.InstitutionMethodDeveloper institutionMethodDeveloper
            )
        {
            DevelopedMethodEdge = new InstitutionDevelopedMethodEdge(institutionMethodDeveloper);
            MethodDeveloperEdge = new InstitutionMethodDeveloperEdge(institutionMethodDeveloper);
        }

        public RemoveInstitutionMethodDeveloperPayload(
            IReadOnlyCollection<RemoveInstitutionMethodDeveloperError> errors
            )
        {
            Errors = errors;
        }

        public RemoveInstitutionMethodDeveloperPayload(
            RemoveInstitutionMethodDeveloperError error
            )
            : this(new[] { error })
        {
        }
    }
}