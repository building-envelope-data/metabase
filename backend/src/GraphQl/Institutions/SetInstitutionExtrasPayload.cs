using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class SetInstitutionExtrasPayload
    : InstitutionPayload<SetInstitutionExtrasError>
{
    public SetInstitutionExtrasPayload(
        Institution institution
    )
        : base(institution)
    {
    }

    public SetInstitutionExtrasPayload(
        SetInstitutionExtrasError error
    )
        : base(error)
    {
    }

    public SetInstitutionExtrasPayload(
        IReadOnlyCollection<SetInstitutionExtrasError> errors
    )
        : base(errors)
    {
    }
}