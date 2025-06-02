using System;
using OpenIddict.EntityFrameworkCore.Models;

namespace Metabase.Data.OpenIdConnect;

public sealed class OpenIdConnectToken : OpenIddictEntityFrameworkCoreToken<Guid, OpenIdConnectApplication, OpenIdConnectAuthorization>
{
}