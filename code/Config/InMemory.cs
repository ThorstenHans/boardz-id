using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;

namespace Boardz.Id.Config
{
    public class InMemory
    {
        
        public static List<TestUser> GetUsers(IConfiguration config)
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = config.GetValue<string>("subject"),
                    Username = config.GetValue<string>("userName"),
                    Password = config.GetValue<string>("password")
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(IConfiguration config)
        {
            yield return new ApiResource(config.GetValue<string>("apiScopeName"), "API");
        }

        public static IEnumerable<Client> GetClients(IConfiguration config)
        {
            yield return new Client
            {
                AccessTokenType = AccessTokenType.Jwt,
                Enabled = true,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AccessTokenLifetime = 28800,
                ClientId = config.GetValue<string>("clientId"),
                ClientSecrets =
                {
                    new Secret(config.GetValue<string>("clientSecret").Sha256())
                },
                AllowedScopes = { config.GetValue<string>("apiScopeName")}
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            yield return new IdentityResources.OpenId();
            yield return new IdentityResources.Profile();
        }
    }
}
