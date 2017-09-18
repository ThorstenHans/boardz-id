using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Boardz.Id.Config
{
    public class InMemory
    {
        
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = System.Environment.GetEnvironmentVariable("IdSrvSubject"),
                    Username = System.Environment.GetEnvironmentVariable("IdSrvUserName"),
                    Password = System.Environment.GetEnvironmentVariable("IdSrvPassword")
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource(System.Environment.GetEnvironmentVariable("IdSrvApiScopeName"), "API");
        }

        public static IEnumerable<Client> GetClients()
        {
            yield return new Client
            {
                AccessTokenType = AccessTokenType.Jwt,
                Enabled = true,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientId = System.Environment.GetEnvironmentVariable("IdSrvClientId"),
                ClientSecrets =
                {
                    new Secret(System.Environment.GetEnvironmentVariable("IdSrvClientSecret").Sha256())
                },
                AllowedScopes = {System.Environment.GetEnvironmentVariable("IdSrvApiScopeName")}
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            yield return new IdentityResources.OpenId();
            yield return new IdentityResources.Profile();
        }
    }
}
