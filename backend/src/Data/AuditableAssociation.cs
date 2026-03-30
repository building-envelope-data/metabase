using NodaTime;

namespace Metabase.Data;

public abstract class AuditableAssociation
: Association, IAuditable
{
    public OffsetDateTime CreatedAt { get; set; }
    public OffsetDateTime UpdatedAt { get; set; }
}
