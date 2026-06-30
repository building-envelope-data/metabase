namespace Metabase.Data;

public interface IAssociation
{
    // Configured via `[Timestamp]` in `Association`
    public uint Version { get; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}