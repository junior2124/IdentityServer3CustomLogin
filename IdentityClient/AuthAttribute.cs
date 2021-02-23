using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace IdentityClient
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]

    public class AuthAttribute : AuthorizeAttribute
    {
        public string UserClaimRoles { get; set; }  //comma string of claims

        public bool IsAPI { get; set; }

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

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //Intercept results where person is authenticated but still doesn't have permissions
            if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (IsAPI)
                {

                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    filterContext.HttpContext.Response.End();
                    return;

                    //filterContext.Result = new JsonResult
                    //{
                    //    Data = new { Message = "Your session has died a terrible and gruesome death" },
                    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    //};
                    //filterContext.HttpContext.Response.StatusCode = 401;
                    //filterContext.HttpContext.Response.StatusDescription = "Humans and robots must authenticate";
                    //filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;

                //    filterContext.RequestContext.HttpContext.Response.ClearContent();
                //    filterContext.Result = new HttpStatusCodeResult(401);
                //    return;
                }

                filterContext.Result = new RedirectResult("/Home");
                return;
            }

            //context.Result = new RedirectToRouteResult(
            //                       new RouteValueDictionary
            //                       {
            //                           { "action", "Contact" },
            //                           { "controller", "Home" }
            //                       }); 

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}