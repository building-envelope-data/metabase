using System;

namespace Metabase.Data;

public abstract class AuditableAssociation
: Association, IAuditable
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}