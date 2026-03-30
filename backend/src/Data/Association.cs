namespace Metabase.Data;

public abstract class Association
{
    // Configured via `IsRowVersion` in `ApplicationDbContext` instead of the annotation
    // [Timestamp]
    public uint Version { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}