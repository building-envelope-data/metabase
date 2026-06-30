using System;

namespace Metabase.Data;

public abstract class Entity
    : IEntity
{
    public Entity()
    {
    }

    public Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; init; }

    // [NotMapped]
    // public Guid Uuid { get => Id; }

    // Configured via `IsRowVersion` in `ApplicationDbContext` instead of the annotation
    // [Timestamp]
    public uint Version { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}