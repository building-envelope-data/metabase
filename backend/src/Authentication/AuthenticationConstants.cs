namespace Metabase.Authentication;

public static class AuthenticationConstants
{
    // `IdentityConstants.ApplicationScheme` is not a constant but only read-only. It can thus not
    // be used in the `Authorize` attribute. See the corresponding issue
    // https://github.com/dotnet/aspnetcore/issues/20122 and un-merged pull request https://github.com/dotnet/aspnetcore/pull/21343/files
    public const string IdentityConstantsApplicationScheme = "Identity.Application";

    public const string IdentityAndCookieAndBearerTokenAuthenticationScheme = "Metabase.Bearer";
}