using Metabase.Data;

namespace Metabase.GraphQl.Institutions;

public sealed class CreateInstitutionPayload
    : InstitutionPayload<CreateInstitutionError>
{
    public CreateInstitutionPayload(
        Institution institution
    )
        : base(institution)
    {
    }

    public CreateInstitutionPayload(
        CreateInstitutionError error
    )
        : base(error)
    {
    }
}