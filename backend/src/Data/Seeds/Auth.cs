using IdentityServer4.Models;
using IdentityServer4;
using System.Collections.Generic;

// TODO Fine proper place and name!
namespace Icon.Data.Seeds
{
    public static class Auth
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResources.OpenId(),
                new IdentityResources.Phone(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
              new ApiResource(Configuration.Auth.ApiName, "Icon")
              {
                ApiSecrets = { new Secret(Configuration.Auth.ApiSecret.Sha256()) }
              }
            };
        }

        // TODO Add a client that corresponds to the following configuration in `appsettings.json`
        // "IdentityServer": {
        //   "Clients": {
        //     "Icon": {
        //       "Profile": "IdentityServerSPA"
        //     }
        //   }
        // },
        // and the following in `appsettings.Development.json`
        // "IdentityServer": {
        //   "Key": {
        //     "Type": "Development"
        //   }
        // },
        // see https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-3.0#appsettingsjson
        // and https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-3.0#other-configuration-options

        // https://identityserver4.readthedocs.io/en/latest/topics/grant_types.html
        public static IEnumerable<Client> GetClients()
        {
            return new Client[]
            {
              // OpenID Connect hybrid flow client (MVC)
              new Client
              {
                ClientId = "mvc",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Hybrid,

                ClientSecrets =
                {
                  new Secret("secret".Sha256())
                },

                RedirectUris = { "https://localhost:5001/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                AllowedScopes =
                {
                  IdentityServerConstants.StandardScopes.Address,
                  IdentityServerConstants.StandardScopes.Email,
                  IdentityServerConstants.StandardScopes.OpenId,
                  IdentityServerConstants.StandardScopes.Phone,
                  IdentityServerConstants.StandardScopes.Profile,
                  Configuration.Auth.ApiName
                },

                AllowOfflineAccess = true
              },
              // JavaScript Client
              new Client
              {
                ClientId = "js",
                ClientName = "JavaScript Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,

                RedirectUris = { "https://localhost:5001/callback.html" },
                PostLogoutRedirectUris = { "https://localhost:5001/index.html" },
                AllowedCorsOrigins = { "https://localhost:5001" },

                AllowedScopes =
                {
                  IdentityServerConstants.StandardScopes.Address,
                  IdentityServerConstants.StandardScopes.Email,
                  IdentityServerConstants.StandardScopes.OpenId,
                  IdentityServerConstants.StandardScopes.Phone,
                  IdentityServerConstants.StandardScopes.Profile,
                  Configuration.Auth.ApiName
                }
              },
              // https://identityserver4.readthedocs.io/en/latest/topics/secrets.html#beyond-shared-secrets
              new Client
              {
                ClientId = "client.jwt",
                ClientSecrets =
                {
                  new Secret
                  {
                    Type = IdentityServerConstants.SecretTypes.X509CertificateBase64,
                    Value = "MIIDATCCAe2gAwIBAgIQoHUYAquk9rBJcq8W+F0FAzAJBgUrDgMCHQUAMBIxEDAOBgNVBAMTB0RldlJvb3QwHhcNMTAwMTIwMjMwMDAwWhcNMjAwMTIwMjMwMDAwWjARMQ8wDQYDVQQDEwZDbGllbnQwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDSaY4x1eXqjHF1iXQcF3pbFrIbmNw19w/IdOQxbavmuPbhY7jX0IORu/GQiHjmhqWt8F4G7KGLhXLC1j7rXdDmxXRyVJBZBTEaSYukuX7zGeUXscdpgODLQVay/0hUGz54aDZPAhtBHaYbog+yH10sCXgV1Mxtzx3dGelA6pPwiAmXwFxjJ1HGsS/hdbt+vgXhdlzud3ZSfyI/TJAnFeKxsmbJUyqMfoBl1zFKG4MOvgHhBjekp+r8gYNGknMYu9JDFr1ue0wylaw9UwG8ZXAkYmYbn2wN/CpJl3gJgX42/9g87uLvtVAmz5L+rZQTlS1ibv54ScR2lcRpGQiQav/LAgMBAAGjXDBaMBMGA1UdJQQMMAoGCCsGAQUFBwMCMEMGA1UdAQQ8MDqAENIWANpX5DZ3bX3WvoDfy0GhFDASMRAwDgYDVQQDEwdEZXZSb290ghAsWTt7E82DjU1E1p427Qj2MAkGBSsOAwIdBQADggEBADLje0qbqGVPaZHINLn+WSM2czZk0b5NG80btp7arjgDYoWBIe2TSOkkApTRhLPfmZTsaiI3Ro/64q+Dk3z3Kt7w+grHqu5nYhsn7xQFAQUf3y2KcJnRdIEk0jrLM4vgIzYdXsoC6YO+9QnlkNqcN36Y8IpSVSTda6gRKvGXiAhu42e2Qey/WNMFOL+YzMXGt/nDHL/qRKsuXBOarIb++43DV3YnxGTx22llhOnPpuZ9/gnNY7KLjODaiEciKhaKqt/b57mTEz4jTF4kIg6BP03MUfDXeVlM1Qf1jB43G2QQ19n5lUiqTpmQkcfLfyci2uBZ8BkOhXr3Vk9HIk/xBXQ="
                  }
                },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
                  Configuration.Auth.ApiName,
                }
              },
              new Client
              {
                ClientId = "swagger",
                ClientName = "Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { "https://localhost:5001/swagger/oauth2-redirect.html" },
                AllowedScopes = { Configuration.Auth.ApiName }
              }
            };
        }
    }
}