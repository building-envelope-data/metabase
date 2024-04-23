namespace Metabase.GraphQl.Institutions;

public sealed class CreateInstitutionPayload
    : InstitutionPayload<CreateInstitutionError>
{
    public CreateInstitutionPayload(
        Data.Institution institution
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