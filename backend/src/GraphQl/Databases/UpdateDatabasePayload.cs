namespace Metabase.GraphQl.Databases;

public sealed class UpdateDatabasePayload
    : DatabasePayload<UpdateDatabaseError>
{
    public UpdateDatabasePayload(
        Data.Database database
    )
        : base(database)
    {
    }

    public UpdateDatabasePayload(
        UpdateDatabaseError error
    )
        : base(error)
    {
    }
}