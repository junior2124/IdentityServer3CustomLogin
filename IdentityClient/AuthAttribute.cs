using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace IdentityClient
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class AuthAttribute : AuthorizeAttribute
    {
        public string UserClaimRoles { get; set; }  //comma string of claims

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isValid = false;

            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            // Get current logged in user's claims
            ClaimsPrincipal claimsPrincipal = (ClaimsPrincipal)HttpContext.Current.User;
            var roleClaims = claimsPrincipal.Claims.Where(x => x.Type == "Role").ToList();
            List<string> userClaimRolesList = UserClaimRoles.Split(',').ToList();

            if (claimsPrincipal != null && claimsPrincipal.Identities.Count() > 0  && claimsPrincipal.Claims != null)
            {
                foreach (string userClaimRoleVal in userClaimRolesList)
                {
                    var validClaim = roleClaims.Where(x => x.Value == userClaimRoleVal).ToList();
                    
                    if (validClaim.Count > 0)
                    {
                        isValid = true;
                        break;
                    }
                }
            }

            return base.AuthorizeCore(httpContext) && isValid;
        }
     }
}