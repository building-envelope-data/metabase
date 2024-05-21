using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Databases;

public abstract class DatabasePayload<TDatabaseError>
    : Payload
    where TDatabaseError : IUserError
{
    protected DatabasePayload(
        Database database
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
        Database database,
        IReadOnlyCollection<TDatabaseError> errors
    )
    {
        Database = database;
        Errors = errors;
    }

    protected DatabasePayload(
        Database database,
        TDatabaseError error
    )
        : this(
            database,
            new[] { error }
        )
    {
    }

    public Database? Database { get; }
    public IReadOnlyCollection<TDatabaseError>? Errors { get; }
}