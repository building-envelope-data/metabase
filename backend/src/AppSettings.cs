// Inspired by https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited

namespace Metabase;

public sealed record AppSettings
{
    public string Host { get; init; } = "";
    public string TestlabSolarFacadesHost { get; init; } = "";
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

public sealed record LoggingSettings(
    bool EnableSensitiveDataLogging = false
);

public sealed record JsonWebTokenSettings(
    string EncryptionCertificatePassword = "",
    string SigningCertificatePassword = ""
);

public sealed record EmailSettings(
    string SmtpHost = "",
    int SmtpPort = 0
);

public sealed record DatabaseSettings(
    string ConnectionString = "",
    string SchemaName = ""
);