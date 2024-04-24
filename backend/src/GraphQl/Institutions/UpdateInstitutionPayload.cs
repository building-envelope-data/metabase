using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class UpdateInstitutionPayload
    : InstitutionPayload<UpdateInstitutionError>
{
    public UpdateInstitutionPayload(
        Institution institution
    )
        : base(institution)
    {
    }

    public UpdateInstitutionPayload(
        UpdateInstitutionError error
    )
        : base(error)
    {
    }
}