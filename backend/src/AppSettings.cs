// Inspired by https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited

using System;
using Microsoft.Extensions.Hosting;

namespace Metabase;

public sealed record AppSettings
{
    private const string GraphQlPathSegment = "/graphql/";
    private const string WwwSubdomain = "www.";

    public string Host { get; init; } = "";
    public Uri HostUri => new(Host, UriKind.Absolute);
    public Uri NonWwwHostUri =>
        HostUri.Host.StartsWith(WwwSubdomain, StringComparison.OrdinalIgnoreCase)
        ? new UriBuilder(HostUri)
        {
            Host = HostUri.Host[WwwSubdomain.Length..]
        }.Uri
        : HostUri;
    public Uri GraphQlEndpoint => new UriBuilder(HostUri) { Path = GraphQlPathSegment }.Uri;
    public string TestlabSolarFacadesHost { get; init; } = "";
    public Uri TestlabSolarFacadesHostUri => new(TestlabSolarFacadesHost, UriKind.Absolute);
    public string BootstrapUserPassword { get; init; } = "";
    public string OpenIdConnectClientSecret { get; init; } = "";
    public string TestlabSolarFacadesOpenIdConnectClientSecret { get; init; } = "";
    public string IgsdbOpenIdConnectClientSecret { get; init; } = "";
    public string IgsdbApiToken { get; init; } = "";
    public LoggingSettings Logging { get; init; } = new();
    public JsonWebTokenSettings JsonWebToken { get; init; } = new();
    public EmailSettings Email { get; init; } = new();
    public DatabaseSettings Database { get; init; } = new();
};

public sealed record LoggingSettings
{
    public bool EnableSensitiveDataLogging { get; init; }
};

public sealed record JsonWebTokenSettings
{
    public string EncryptionCertificatePassword { get; init; } = "";
    public string SigningCertificatePassword { get; init; } = "";
};

public sealed record EmailSettings
{
    public string SmtpHost { get; init; } = "";
    public int SmtpPort { get; init; }
};

public sealed record DatabaseSettings
{
    public string ConnectionString { get; set; } = "";
    public string SchemaName { get; init; } = "";
};