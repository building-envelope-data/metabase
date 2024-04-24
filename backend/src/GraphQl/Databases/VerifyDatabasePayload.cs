using Metabase.Data;

namespace Metabase.GraphQl.Databases;

public sealed class VerifyDatabasePayload
    : DatabasePayload<VerifyDatabaseError>
{
    public VerifyDatabasePayload(
        Database database
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