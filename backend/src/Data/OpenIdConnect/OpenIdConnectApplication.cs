using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.Data.OpenIdConnect;

public sealed class OpenIdConnectApplication
    : OpenIddictEntityFrameworkCoreApplication<Guid, OpenIdConnectAuthorization, OpenIdConnectToken>,
      IEntity
{
    public Guid OwnerId { get; set; }

    [InverseProperty(nameof(Institution.OpenIdConnectApplications))]
    public Institution Owner { get; set; } = null!;

    [Timestamp]
    public uint Version { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}