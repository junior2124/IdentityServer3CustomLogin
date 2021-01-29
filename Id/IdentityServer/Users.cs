using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using System.Collections.Generic;
using System.Security.Claims;

namespace Id.IdentityServer
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "bob",
                    Password = "secret",
                    Subject = "1",

                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Bob"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Smith"),
                        new Claim(Constants.ClaimTypes.Role, "Admin"),
                        new Claim(Constants.ClaimTypes.Role, "Member"),
                        new Claim(Constants.ClaimTypes.Role, "Users"),
                        new Claim(Constants.ClaimTypes.Email, "bob.secret@gmail.com"),

                    }
                },
                new InMemoryUser
                {
                    Username = "junior2124@msn.com",
                    Password = "secret",
                    Subject = "44313d7b-f5c9-479a-83f8-5ab1016647c1",

                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Junior"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Sanchez"),
                        new Claim(Constants.ClaimTypes.Role, "Admin"),
                        new Claim(Constants.ClaimTypes.Role, "Member"),
                        new Claim(Constants.ClaimTypes.Role, "Users"),
                        new Claim("rememberMe", "N"),
                        new Claim(Constants.ClaimTypes.Email, "junior2124@msn.com"),

                    }
                },
            };
        }
    }
}