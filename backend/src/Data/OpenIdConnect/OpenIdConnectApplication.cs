using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.Data.OpenIdConnect;

public sealed class OpenIdConnectApplication
    : OpenIddictEntityFrameworkCoreApplication<Guid, OpenIdConnectAuthorization, OpenIdConnectToken>,
      IEntity
{
    public ICollection<InstitutionOpenIdConnectApplication> InstitutionEdges { get; } = [];

    public ICollection<Institution> Institutions { get; } = [];

    [Timestamp]
    public uint Version { get; private set; } // https://www.npgsql.org/efcore/modeling/concurrency.html
}