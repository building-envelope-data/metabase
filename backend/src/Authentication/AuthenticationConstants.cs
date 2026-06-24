namespace Metabase.Authentication;

internal static class AuthenticationConstants
{
    // `IdentityConstants.ApplicationScheme` is not a constant but only read-only. It can thus not
    // be used in the `Authorize` attribute. See the corresponding issue
    // https://github.com/dotnet/aspnetcore/issues/20122 and un-merged pull request https://github.com/dotnet/aspnetcore/pull/21343/files
    internal const string IdentityApplicationScheme = "Identity.Application";

    internal const string IdentityAndCookieAndBearerTokenAuthenticationScheme = "Metabase.Bearer";

    internal const string LoginPath = "/users/login";
}