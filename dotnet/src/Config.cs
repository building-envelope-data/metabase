using IdentityServer4.Models;
using IdentityServer4;
using System.Collections.Generic;

namespace Icon
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                    new IdentityResources.Email(),
                    new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[] {
            new ApiResource("api", "Icon")
          };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new Client[] {
            new Client
            {
              ClientId = "client",

                       // no interactive user, use the clientid/secret for authentication
                       AllowedGrantTypes = GrantTypes.ClientCredentials,

                       // secret for authentication
                       ClientSecrets =
                       {
                         new Secret("secret".Sha256())
                       },

                       // scopes that client has access to
                       AllowedScopes = { "api1" }
            },
              // resource owner password grant client
              new Client
              {
                ClientId = "ro.client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                ClientSecrets =
                {
                  new Secret("secret".Sha256())
                },
                AllowedScopes = { "api1" }
              },
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

                RedirectUris           = { "http://localhost:5002/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                AllowedScopes =
                {
                  IdentityServerConstants.StandardScopes.OpenId,
                  IdentityServerConstants.StandardScopes.Profile,
                  "api1"
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

                RedirectUris =           { "http://localhost:5003/callback.html" },
                PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                AllowedCorsOrigins =     { "http://localhost:5003" },

                AllowedScopes =
                {
                  IdentityServerConstants.StandardScopes.OpenId,
                  IdentityServerConstants.StandardScopes.Profile,
                  "api1"
                }
              }
          };
        }
    }
}