using System;
using System.Collections.Generic;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.Data.OpenIdConnect;

public class OpenIdConnectApplication : OpenIddictEntityFrameworkCoreApplication<Guid, OpenIdConnectAuthorization, OpenIdConnectToken>
{
    public ICollection<InstitutionOpenIdConnectApplication> InstitutionEdges { get; } = [];

    public ICollection<Institution> Institutions { get; } = [];
}