using System;
using Metabase.Data.OpenIdConnect;

namespace Metabase.Data;

public sealed class InstitutionOpenIdConnectApplication
{
    public Guid InstitutionId { get; set; }
    public Institution Institution { get; set; } = default!;

    public Guid ApplicationId { get; set; }
    public OpenIdConnectApplication Application { get; set; } = default!;
}