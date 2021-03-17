namespace Metabase.GraphQl.Users
{
    public sealed class UserRepresentedInstitutionConnection
        : Connection<Data.User, Data.InstitutionRepresentative, UserRepresentedInstitutionsByUserIdDataLoader, UserRepresentedInstitutionEdge>
    {
        public UserRepresentedInstitutionConnection(
            Data.User subject
        )
            : base(
                subject,
                x => new UserRepresentedInstitutionEdge(x)
                )
        {
        }
    }
}