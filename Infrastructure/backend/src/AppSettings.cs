// Inspired by https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited

namespace Infrastructure
{
    public sealed class AppSettings
    {
        public string Host { get; set; }
        = "https://icon.com";

        public LoggingSettings Logging { get; set; }
        = new LoggingSettings();

        public sealed class LoggingSettings
        {
            public bool EnableSensitiveDataLogging { get; set; }
            = false;
        }

        public DatabaseSettings Database { get; set; }
        = new DatabaseSettings();

        public sealed class DatabaseSettings
        {
            public string ConnectionString { get; set; }
            = "Host=database;Port=5432;Database=icon;";

            public string SchemaName { get; set; }
            = "metabase";
        }
    }
}