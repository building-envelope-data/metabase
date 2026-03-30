using System;
using NodaTime;

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

    public OffsetDateTime CreatedAt { get; set; }
    public OffsetDateTime UpdatedAt { get; set; }
}
