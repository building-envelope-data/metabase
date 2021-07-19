namespace Metabase.GraphQl.Users
{
    public sealed class UserRepresentedInstitutionConnection
        : ForkingConnection<Data.User, Data.InstitutionRepresentative, PendingUserRepresentedInstitutionsByUserIdDataLoader, UserRepresentedInstitutionsByUserIdDataLoader, UserRepresentedInstitutionEdge>
    {
        public UserRepresentedInstitutionConnection(
            Data.User subject,
            bool pending
        )
            : base(
                subject,
                pending,
                x => new UserRepresentedInstitutionEdge(x)
                )
        {
        }
    }
}