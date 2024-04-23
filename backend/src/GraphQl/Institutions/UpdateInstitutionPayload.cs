namespace Metabase.GraphQl.Institutions
{
    public sealed class UpdateInstitutionPayload
        : InstitutionPayload<UpdateInstitutionError>
    {
        public UpdateInstitutionPayload(
            Data.Institution institution
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
}