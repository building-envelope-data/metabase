using System;

namespace Metabase.Data;

public abstract class AuditableEntity
: Entity, IAuditable
{
    public AuditableEntity()
    : base()
    {
    }

    public AuditableEntity(Guid id)
    : base(id)
    {
    }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}