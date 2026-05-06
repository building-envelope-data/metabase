using System;
using NodaTime;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.Data.OpenIdConnect;

public sealed class OpenIdConnectAuthorization
    : OpenIddictEntityFrameworkCoreAuthorization<Guid, OpenIdConnectApplication, OpenIdConnectToken>,
      IEntity,
      IAuditable
{
    // `createdAt` could be an alias of `creationDate`
    public OffsetDateTime CreatedAt { get; set; }
    public OffsetDateTime UpdatedAt { get; set; }

    // Configured via `IsRowVersion` in `ApplicationDbContext` instead of the annotation
    // [Timestamp]
    public uint Version { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}
