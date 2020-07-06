// Inspired by https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited

namespace Metabase
{
    public class AppSettings
    {
        public string Host { get; set; }
        = "https://icon.com";

        public LoggingSettings Logging { get; set; }
        = new LoggingSettings();

        public class LoggingSettings
        {
            public bool EnableSensitiveDataLogging { get; set; }
            = false;
        }

        public DatabaseSettings Database { get; set; }
        = new DatabaseSettings();

        public class DatabaseSettings
        {
            public string ConnectionString { get; set; }
            = "Host=database;Port=5432;Database=icon;";

            public SchemaNameSettings SchemaName { get; set; }
            = new SchemaNameSettings();

            public class SchemaNameSettings
            {
                public string Application { get; set; }
                = "application";

                public string EventStore { get; set; }
                = "event_store";

                public string IdentityServerPersistedGrant { get; set; }
                = "persisted_grant";

                public string IdentityServerConfiguration { get; set; }
                = "configuration";
            }
        }
    }
}