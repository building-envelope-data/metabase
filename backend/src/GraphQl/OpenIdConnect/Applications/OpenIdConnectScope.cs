using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect.Applications;

[SuppressMessage("Naming", "CA1707")]
public enum OpenIdConnectScope
{
    ADDRESS,
    EMAIL,
    OFFLINE_ACCESS,
    OPEN_ID,
    PHONE,
    PROFILE,
    ROLES,
    READ_API,
    WRITE_API,
    ADMINISTRATE_API,
    VERIFY_API,
    MANAGE_DATABASE_API,
    MANAGE_GNU_PG_API,
    MANAGE_INSTITUTION_REPRESENTATIVE_API,
    MANAGE_OPEN_ID_CONNECT_API,
    MANAGE_USER_API,
}