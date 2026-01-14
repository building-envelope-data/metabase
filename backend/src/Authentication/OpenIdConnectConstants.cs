using System;

namespace Metabase.Authentication;

public static class OpenIdConnectConstants
{
    public static readonly TimeSpan AccessAndIdentityTokenLifetime = TimeSpan.FromHours(1);

    public const string MetabaseRegistrationId = "metabase";
    public const string MetabaseClientId = "metabase";
    public const string MetabaseQuartzSchedulerId = "metabase";
    public const string AuthorizationHeaderBearer = "Bearer";
}