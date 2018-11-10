using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;


namespace PDJaya.Identity
{
    public class Config
    {
        public Config()
        {
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                 new IdentityResources.Email(),
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "admin",
                    Password = "admin",
                    Claims = new []
                    {
                        new Claim("name", "admin"),
                        new Claim("website", "https://pdjaya.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "user1",
                    Password = "password",
                    Claims = new []
                    {
                        new Claim("name", "user1"),
                        new Claim("website", "https://pdjaya.com")
                    }
                }
            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("masterdataapi", "Master Data API"),
                new ApiResource("transactionapi", "Transaction Data API"),

            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
    {
                new Client
                {
                    ClientId = "serviceapp",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("4515647407".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "masterdataapi", "transactionapi" }
                },
                new Client
                {
                    ClientId = "webapp1",ClientName="Web App send User & Password",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("web123".Sha256())
                    },
                    AllowedScopes = { "masterdataapi", "transactionapi" }
                },
                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    /*
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = { "http://52.230.8.37/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://52.230.8.37/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                    */
                    ClientId = "webapp2",
                    ClientName = "web with openid",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("web123".Sha256())
                    },

                    RedirectUris           = { "http://pdjayaauthapi.azurewebsites.net/signin-oidc" },
                    PostLogoutRedirectUris = { "http://pdjayaauthapi.azurewebsites.net/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "masterdataapi",
                        "transactionapi"
                    },
                    AllowOfflineAccess = true
                }
    };
        }


    }
}
