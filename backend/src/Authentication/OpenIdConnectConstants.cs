using System;
using System.Security.Cryptography.X509Certificates;

namespace Metabase.Authentication;

public static class OpenIdConnectConstants
{
    public static readonly TimeSpan AccessAndIdentityTokenLifetime = TimeSpan.FromHours(1);
    public static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(1);

    public const string AuthorizationHeaderBearer = "Bearer";
    public const string MetabaseQuartzSchedulerId = "metabase";

    public const StoreName CertificateStoreName = StoreName.My;
    public const StoreLocation CertificateStoreLocation = StoreLocation.CurrentUser;

    public static class Server
    {
        public const string SigningSubjectDistinguishedName = $"CN=Metabase OpenId Connect Server Signing Certificate";
        public const string EncryptionSubjectDistinguishedName = $"CN=Metabase OpenId Connect Server Encryption Certificate";
    }

    public static class Client
    {
        public const string MetabaseRegistrationId = "metabase";
        public const string MetabaseClientId = "metabase";
        public const string SigningSubjectDistinguishedName = $"CN=Metabase OpenId Connect Client Signing Certificate";
        public const string EncryptionSubjectDistinguishedName = $"CN=Metabase OpenId Connect Client Encryption Certificate";
    }
}