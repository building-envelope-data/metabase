namespace Metabase.GraphQl.Users
{
  public abstract class UserPayload
    : GraphQl.Payload
    {
        public Data.User? User { get; }

        protected UserPayload()
        {
        }

        protected UserPayload(
            Data.User? user
            )
        {
            User = user;
        }
    }
}
