using Metabase.Data;

namespace Metabase.GraphQl.Databases;

public sealed class UpdateDatabasePayload
    : DatabasePayload<UpdateDatabaseError>
{
    public UpdateDatabasePayload(
        Database database
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