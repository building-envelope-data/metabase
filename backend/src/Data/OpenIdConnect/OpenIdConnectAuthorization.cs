using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.Data.OpenIdConnect;

public sealed class OpenIdConnectAuthorization : OpenIddictEntityFrameworkCoreAuthorization<Guid, OpenIdConnectApplication, OpenIdConnectToken>
{
}