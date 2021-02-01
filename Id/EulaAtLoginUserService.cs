using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Id.Models;
using IdentityServer3.Core;
using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;

namespace Id
{
    public class EulaAtLoginUserService : UserServiceBase
    {
        private ISSDBContext _context;

        public EulaAtLoginUserService() 
        {
            _context = new ISSDBContext();
        }

        public static List<User> Users = new List<User>()
        {
            new User{
                Id = 818727,
                Username = "alice",
                Password = "alice",
                AcceptedEula = false,
                UserClaims = new List<UserClaim>{
                    new UserClaim
                    {
                        ClaimId = 1,
                        ClaimType = "GivenName",
                        Value = "Alice"
                    },
                    new UserClaim
                    {
                        ClaimId = 2,
                        ClaimType = "FamilyName",
                        Value = "Smith"
                    },
                    new UserClaim
                    {
                        ClaimId = 3,
                        ClaimType = "Role",
                        Value = "Admin"
                    },
                     new UserClaim
                    {
                        ClaimId = 4,
                        ClaimType = "Email",
                        Value = "AliceSmith@email.com"
                    },
                }
            },
        };

        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            //var user = Users.SingleOrDefault(x => x.Username == context.UserName && x.Password == context.Password);
            var user = _context.Users.SingleOrDefault(x => x.Username == context.UserName && x.Password == context.Password);
            if (user != null)
            {
                if (user.AcceptedEula)
                {
                    context.AuthenticateResult = new AuthenticateResult(user.Id.ToString(), user.Username);
                }
                else
                {
                    context.AuthenticateResult = new AuthenticateResult("~/eula", user.Id.ToString(), user.Username);
                }
            }
            return Task.FromResult(0);
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // issue the claims for the user
            //var user = Users.SingleOrDefault(x => x.Id.ToString() == context.Subject.GetSubjectId());
            var subId = context.Subject.GetSubjectId();
            var user = _context.Users.SingleOrDefault(x => x.Id.ToString() == subId);

            if (user != null)
            {
                List<Claim> ClaimsList = new List<Claim>();

                //new Claim(Constants.ClaimTypes.GivenName, "Alice")
                foreach (var claim in user.UserClaims)
                {
                    var vClaim = new Claim(claim.ClaimType,claim.Value);
                    ClaimsList.Add(vClaim);
                }
                //context.IssuedClaims = user.Claims.Where(x => context.RequestedClaimTypes.Contains(x.Type));
                context.IssuedClaims = ClaimsList;
            }
            return Task.FromResult(0);
        }
    }
}
