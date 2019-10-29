// Inspired by https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited

using Microsoft.Extensions.Configuration;

namespace Icon
{
    public class AppSettings
    {
        public string Host { get; set; }

        public LoggingSettings Logging { get; set; }

        public class LoggingSettings
        {
            public bool EnableSensitiveDataLogging { get; set; }
        }

        public DatabaseSettings Database { get; set; }

        public class DatabaseSettings
        {
            public string ConnectionString { get; set; }

            public SchemaNameSettings SchemaName { get; set; }

            public class SchemaNameSettings
            {
                public string Application { get; set; }
                public string EventStore { get; set; }
                public string IdentityServerPersistedGrant { get; set; }
                public string IdentityServerConfiguration { get; set; }
            }
        }
    }
}