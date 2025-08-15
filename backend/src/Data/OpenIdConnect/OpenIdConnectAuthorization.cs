using System;
using System.ComponentModel.DataAnnotations;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.Data.OpenIdConnect;

public sealed class OpenIdConnectAuthorization
    : OpenIddictEntityFrameworkCoreAuthorization<Guid, OpenIdConnectApplication, OpenIdConnectToken>,
      IEntity
{
    [Timestamp]
    public uint Version { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}