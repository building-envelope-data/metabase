// Inspired by https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited

namespace Metabase
{
    public sealed class AppSettings
    {
        public string Host { get; set; }
        = "";

        public LoggingSettings Logging { get; set; }
        = new LoggingSettings();

        public sealed class LoggingSettings
        {
            public bool EnableSensitiveDataLogging { get; set; }
            = false;
        }

        public JsonWebTokenSettings JsonWebToken { get; set; }
        = new JsonWebTokenSettings();

        public sealed class JsonWebTokenSettings
        {
            public string EncryptionCertificatePassword { get; set; }
            = "";

            public string SigningCertificatePassword { get; set; }
            = "";
        }

        public string TestlabSolarFacadesOpenIdConnectClientSecret { get; set; }
        = "";

        public DatabaseSettings Database { get; set; }
        = new DatabaseSettings();

        public sealed class DatabaseSettings
        {
            public string ConnectionString { get; set; }
            = "";

            public string SchemaName { get; set; }
            = "";
        }
    }
}