using System;
using System.ComponentModel.DataAnnotations.Schema;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.Data.OpenIdConnect;

public sealed class OpenIdConnectApplication
: OpenIddictEntityFrameworkCoreApplication<Guid, OpenIdConnectAuthorization, OpenIdConnectToken>,
  IEntity,
  IAuditable,
  IOpenIdConnectSubject
{
    public Guid OwnerId { get; set; }

    [InverseProperty(nameof(Institution.OpenIdConnectApplications))]
    public Institution Owner { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // Configured via `IsRowVersion` in `ApplicationDbContext` instead of the annotation
    // [Timestamp]
    public uint Version { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}