using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace Id.IdentityServer
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    ClientName = "OpenIDConnect",
                    ClientId = "myMVC",
                    Flow = Flows.Implicit,

                    RedirectUris = new List<string>
                    {
                        //"https://localhost:60879/",
                        "http://localhost:60879/Sitefinity/Authenticate/OpenID/signin-idserver3"
                    },
                    //AllowAccessToAllScopes = true
                   AllowedScopes = new List<string>
                    {
                        "openid", "profile", "email", "rememberMe", "roles"
                    }
                },
                new Client
                {
                    ClientName = "MVC Client",
                    ClientId = "mvc",
                    Flow = Flows.Implicit,

                    RedirectUris = new List<string>
                    {
                        "https://localhost:44367/"
                    },
                    AllowAccessToAllScopes = true

                },
                new Client
                {
                    ClientName = "MVC Client (service communication)",
                    ClientId = "mvc_service",
                    Flow = Flows.ClientCredentials,

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "sampleApi"
                    }
                }
            };
        }
    }
}