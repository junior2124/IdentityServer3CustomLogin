using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace Id.IdentityServer
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            var scopes = new List<Scope>
            {
                new Scope
                {
                    Enabled = true,
                    Name = "roles",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role"),
                    }
                },
                new Scope
                {
                    Enabled = true,
                    Name = "rememberMe",
                    Type = ScopeType.Identity,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("rememberMe"),
                    }
                },
                new Scope
                {
                    Enabled = true,
                    DisplayName = "Sample API",
                    Name = "sampleApi",
                    Description = "Access to a sample API",
                    Type = ScopeType.Resource,

                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
                ////////////////////////
                    // resource scopes
                    ////////////////////////

                    new Scope
                    {
                        Name = "read",
                        DisplayName = "Read data",
                        Type = ScopeType.Resource,
                        Emphasize = false,

                        ScopeSecrets = new List<Secret>
                        {
                            new Secret("secret".Sha256())
                        }
                    },
                    new Scope
                    {
                        Name = "write",
                        DisplayName = "Write data",
                        Type = ScopeType.Resource,
                        Emphasize = true,

                        ScopeSecrets = new List<Secret>
                        {
                            new Secret("secret".Sha256())
                        }
                    },
                    new Scope
                    {
                        Name = "idmgr",
                        DisplayName = "IdentityManager",
                        Type = ScopeType.Resource,
                        Emphasize = true,
                        ShowInDiscoveryDocument = false,

                        Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim(Constants.ClaimTypes.Name),
                            new ScopeClaim(Constants.ClaimTypes.Role)
                        }
                    }
            };

            scopes.AddRange(StandardScopes.All);

            return scopes;
        }
    }
}