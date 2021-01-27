namespace Metabase.GraphQl.Databases
{
    public sealed class CreateDatabasePayload
      : DatabasePayload<CreateDatabaseError>
    {
        public CreateDatabasePayload(
            Data.Database database
            )
              : base(database)
        {
        }
    }
}