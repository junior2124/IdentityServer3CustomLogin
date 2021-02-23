using Id.Models;
using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using System.Collections.Generic;
using System.Security.Claims;

namespace Id.IdentityServer
{
    public class Users
    {
        public static List<User> Get()
        {
            return new List<User>
            {
                new User{
                Id = 123,
                Username = "alice",
                Password = "alice",
                AcceptedEula = false,
                },
                new User{
                    Id = 890,
                    Username = "bob",
                    Password = "bob",
                    AcceptedEula = false,
                }
            };
        }
    }
}