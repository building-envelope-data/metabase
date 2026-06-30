using System;

namespace Metabase.Data;

public interface IAuditable
{
    // TODO Switch to NodaTime `OffsetDateTime` once there is a `ICursorKeySerializer` implementation for it. Then sorting by `CreatedAt` and `UpdatedAt` with pagination will keep working.
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // soft delete
    // public Instant? DeletedAt { get; set; }
}