using System.Collections.Generic;

namespace Metabase.GraphQl.Databases;

public abstract class DatabasePayload<TDatabaseError>
    : Payload
    where TDatabaseError : IUserError
{
    public Data.Database? Database { get; }
    public IReadOnlyCollection<TDatabaseError>? Errors { get; }

    protected DatabasePayload(
        Data.Database database
    )
    {
        Database = database;
    }

    protected DatabasePayload(
        IReadOnlyCollection<TDatabaseError> errors
    )
    {
        Errors = errors;
    }

    protected DatabasePayload(
        TDatabaseError error
    )
        : this(new[] { error })
    {
    }

    protected DatabasePayload(
        Data.Database database,
        IReadOnlyCollection<TDatabaseError> errors
    )
    {
        Database = database;
        Errors = errors;
    }

    protected DatabasePayload(
        Data.Database database,
        TDatabaseError error
    )
        : this(
            database,
            new[] { error }
        )
    {
    }
}