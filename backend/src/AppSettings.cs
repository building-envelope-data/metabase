// Inspired by https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited

namespace Metabase;

public sealed class AppSettings
{
    public string Host { get; private set; }
        = "";

    public string TestlabSolarFacadesHost { get; private set; }
        = "";

    public LoggingSettings Logging { get; private set; } = new();

    public JsonWebTokenSettings JsonWebToken { get; private set; } = new();

    public EmailSettings Email { get; private set; } = new();

    public string BootstrapUserPassword { get; private set; }
        = "";

    public string OpenIdConnectClientSecret { get; private set; }
        = "";

    public string TestlabSolarFacadesOpenIdConnectClientSecret { get; private set; }
        = "";

    public string IgsdbOpenIdConnectClientSecret { get; private set; }
        = "";

    public string IgsdbApiToken { get; private set; }
        = "";

    public DatabaseSettings Database { get; private set; } = new();

    public sealed class LoggingSettings
    {
        public bool EnableSensitiveDataLogging { get; private set; }
    }

    public sealed class JsonWebTokenSettings
    {
        public string EncryptionCertificatePassword { get; private set; }
            = "";

        public string SigningCertificatePassword { get; private set; }
            = "";
    }

    public sealed class EmailSettings
    {
        public string SmtpHost { get; private set; }
            = "";

        public int SmtpPort { get; private set; }
    }

    public sealed class DatabaseSettings
    {
        public string ConnectionString { get; private set; }
            = "";

        public string SchemaName { get; private set; }
            = "";
    }
}