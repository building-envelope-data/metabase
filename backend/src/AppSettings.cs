// Inspired by https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited

namespace Metabase;

public sealed class AppSettings
{
    public string Host { get; set; }
        = "";

    public string TestlabSolarFacadesHost { get; set; }
        = "";

    public LoggingSettings Logging { get; set; } = new();

    public JsonWebTokenSettings JsonWebToken { get; set; } = new();

    public EmailSettings Email { get; set; } = new();

    public string BootstrapUserPassword { get; set; }
        = "";

    public string OpenIdConnectClientSecret { get; set; }
        = "";

    public string TestlabSolarFacadesOpenIdConnectClientSecret { get; set; }
        = "";

    public DatabaseSettings Database { get; set; } = new();

    public sealed class LoggingSettings
    {
        public bool EnableSensitiveDataLogging { get; set; }
    }

    public sealed class JsonWebTokenSettings
    {
        public string EncryptionCertificatePassword { get; set; }
            = "";

        public string SigningCertificatePassword { get; set; }
            = "";
    }

    public sealed class EmailSettings
    {
        public string SmtpHost { get; set; }
            = "";

        public int SmtpPort { get; set; }
    }

    public sealed class DatabaseSettings
    {
        public string ConnectionString { get; set; }
            = "";

        public string SchemaName { get; set; }
            = "";
    }
}