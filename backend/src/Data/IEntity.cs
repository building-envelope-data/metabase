using Guid = System.Guid;

namespace Metabase.Data;

public interface IEntity
{
    public Guid Id { get; }

    public uint Version { get; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}