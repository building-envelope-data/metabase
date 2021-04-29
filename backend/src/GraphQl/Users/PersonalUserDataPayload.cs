namespace Metabase.GraphQl.Users
{
    public sealed class PersonalUserDataPayload
      : UserPayload<PersonalUserDataError>
    {
        public PersonalUserDataPayload(
            Data.User user
            )
          : base(user)
        {
        }

        public PersonalUserDataPayload(
            PersonalUserDataError error
            )
          : base(error)
        {
        }
    }
}