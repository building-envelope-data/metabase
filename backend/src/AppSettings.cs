// Inspired by https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited

using System;

namespace Metabase;

public sealed record AppSettings
{
    private const string GraphQlPathSegment = "/graphql/";

    public string Host { private get; init; } = "";
    public string Subdomain { private get; init; } = "";
    public Uri Uri => new($"https://{Subdomain}.{Host}", UriKind.Absolute);
    public Uri GraphQlEndpoint => new UriBuilder(Uri) { Path = GraphQlPathSegment }.Uri;

    public string BootstrapUserPassword { get; init; } = "";
    public string OpenIdConnectClientSecret { get; init; } = "";
    public TestlabSolarFacadesSettings TestlabSolarFacades { get; init; } = new();
    public IgsdbSettings Igsdb { get; init; } = new();
    public LoggingSettings Logging { get; init; } = new();
    public EmailSettings Email { get; init; } = new();
    public DatabaseSettings Database { get; init; } = new();
    public OpenTelemetrySettings OpenTelemetry { get; init; } = new();

    public sealed record TestlabSolarFacadesSettings
    {
        public string Host { private get; init; } = "";
        public Uri Uri => new($"https://{Host}", UriKind.Absolute);
        public string OpenIdConnectClientSecret { get; init; } = "";
    };

    public sealed record IgsdbSettings
    {
        public string OpenIdConnectClientSecret { get; init; } = "";
        public string ApiToken { get; init; } = "";
    };

    public sealed record LoggingSettings
    {
        public bool EnableSensitiveDataLogging { get; init; }
    };

    public sealed record EmailSettings
    {
        public string SmtpHost { get; init; } = "";
        public int SmtpPort { get; init; }
    };

    public sealed record DatabaseSettings
    {
        public string Host { get; init; } = "";
        public int Port { get; init; }
        public string Name { get; set; } = "";
        public string UserName { get; init; } = "";
        public string Password { get; init; } = "";
        public string SchemaName { get; init; } = "";
    };

    public sealed record OpenTelemetrySettings
    {
        public string Host { get; init; } = "";
        public int GrpcPort { get; init; }
        public Uri GrpcUri =>
            new UriBuilder(
                scheme: "http",
                host: Host,
                portNumber: GrpcPort
            )
            .Uri;
    };
};