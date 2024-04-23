using System.Collections.Generic;
using Metabase.GraphQl.Methods;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.InstitutionMethodDevelopers
{
    public sealed class ConfirmInstitutionMethodDeveloperPayload
    {
        public InstitutionDevelopedMethodEdge? DevelopedMethodEdge { get; }
        public InstitutionMethodDeveloperEdge? MethodDeveloperEdge { get; }
        public IReadOnlyCollection<ConfirmInstitutionMethodDeveloperError>? Errors { get; }

        public ConfirmInstitutionMethodDeveloperPayload(
            Data.InstitutionMethodDeveloper institutionMethodDeveloper
        )
        {
            DevelopedMethodEdge = new InstitutionDevelopedMethodEdge(institutionMethodDeveloper);
            MethodDeveloperEdge = new InstitutionMethodDeveloperEdge(institutionMethodDeveloper);
        }

        public ConfirmInstitutionMethodDeveloperPayload(
            IReadOnlyCollection<ConfirmInstitutionMethodDeveloperError> errors
        )
        {
            Errors = errors;
        }

        public ConfirmInstitutionMethodDeveloperPayload(
            ConfirmInstitutionMethodDeveloperError error
        )
            : this(new[] { error })
        {
        }
    }
}