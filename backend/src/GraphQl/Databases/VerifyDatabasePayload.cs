namespace Metabase.GraphQl.Databases
{
    public sealed class VerifyDatabasePayload
      : DatabasePayload<VerifyDatabaseError>
    {
        public VerifyDatabasePayload(
            Data.Database database
            )
              : base(database)
        {
        }

        public VerifyDatabasePayload(
            VerifyDatabaseError error
            )
              : base(error)
        {
        }
    }
}