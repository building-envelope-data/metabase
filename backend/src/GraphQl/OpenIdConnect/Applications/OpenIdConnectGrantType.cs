using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[SuppressMessage("Naming", "CA1707")]
public enum OpenIdConnectGrantType
{
    AUTHORIZATION_CODE,
    CLIENT_CREDENTIALS,
    REFRESH_TOKEN,
    TOKEN_EXCHANGE,
}