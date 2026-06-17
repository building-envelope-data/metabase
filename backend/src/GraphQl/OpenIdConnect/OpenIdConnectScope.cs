using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.OpenIdConnect;

[SuppressMessage("Naming", "CA1707")]
public enum OpenIdConnectScope
{
    // The descriptions are used through `EnumExtensions.GetDescription` in the view `Authorize.cshtml`.

    [Display(Description = "Access to your postal address information.")]
    ADDRESS,

    [Display(Description = "Access to your email address.")]
    EMAIL,

    [Display(Description = "Access to your phone number.")]
    PHONE,

    [Display(Description = "Access to your basic profile, for example, name, gender, picture, and so forth.")]
    PROFILE,

    [Display(Description = "Request a long-lived refresh token so the application can continue interacting with APIs after the original access token expired.")]
    OFFLINE_ACCESS,

    [Display(Description = "Request an identity token so the application can know who you are.")]
    OPEN_ID,

    [Display(Description = "Access to your user roles.")]
    ROLES,

    [Display(Description = "Read-only access to resources in your name.")]
    READ_API,

    [Display(Description = "Basic write access to resources in your name.")]
    WRITE_API,

    [Display(Description = "Permission to perform administrative tasks in your name.")]
    ADMINISTRATE_API,

    [Display(Description = "Permission to perform verification tasks in your name.")]
    VERIFY_API,

    [Display(Description = "Permission to perform customer support tasks in your name.")]
    SUPPORT_API,

    [Display(Description = "Authorization to manage databases in your name.")]
    MANAGE_DATABASE_API,

    [Display(Description = "Authorization to manage GnuPG keys in your name.")]
    MANAGE_GNU_PG_API,

    [Display(Description = "Authorization to manage institution representatives in your name.")]
    MANAGE_INSTITUTION_REPRESENTATIVE_API,

    [Display(Description = "Authorization to manage OpenID Connect settings in your name.")]
    MANAGE_OPEN_ID_CONNECT_API,

    [Display(Description = "Authorization to manage your user account.")]
    MANAGE_USER_API,
}