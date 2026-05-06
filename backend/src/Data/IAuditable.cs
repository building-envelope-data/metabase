using NodaTime;

namespace Metabase.Data;

public interface IAuditable
{
    public OffsetDateTime CreatedAt { get; set; }
    public OffsetDateTime UpdatedAt { get; set; }

    // soft delete
    // public Instant? DeletedAt { get; set; }
}
