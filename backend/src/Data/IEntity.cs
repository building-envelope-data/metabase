using Guid = System.Guid;

namespace Metabase.Data;

public interface IEntity
{
    public Guid Id { get; }

    // Configured via `IsRowVersion` in `ApplicationDbContext`
    public uint Version { get; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}