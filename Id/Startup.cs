using Id.IdentityServer;
using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using IdentityServer3.Core.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;
using IdentityServer3.Core.Services.Default;

[assembly: OwinStartup(typeof(Id.Startup))]

namespace Id
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/core", coreApp =>
            {
                var factory = new IdentityServerServiceFactory()
                       // .UseInMemoryUsers(Users.Get()) 
                        .UseInMemoryClients(Clients.Get())
                        .UseInMemoryScopes(Scopes.Get());

                var userService = new EulaAtLoginUserService();

                factory.UserService = new Registration<IUserService>(resolver => userService);

                var viewOptions = new DefaultViewServiceOptions();
                viewOptions.Stylesheets.Add("/Content/Site.css");
                viewOptions.CacheViews = false;
                factory.ConfigureDefaultViewService(viewOptions);

                var options = new IdentityServerOptions
                {
                    SiteName = "IdentityServer3 - Custom Login Page",

                    SigningCertificate = LoadCertificate(),
                    Factory = factory,

                AuthenticationOptions = new IdentityServer3.Core.Configuration.AuthenticationOptions
                    {
                        CookieOptions = new IdentityServer3.Core.Configuration.CookieOptions()
                        {
                            AllowRememberMe = true,
                            SecureMode = CookieSecureMode.Always,
                            RememberMeDuration = TimeSpan.FromDays(30),
                            SlidingExpiration = true
                        },
                    },

                    EventsOptions = new EventsOptions
                    {
                        RaiseSuccessEvents = true,
                        RaiseErrorEvents = true,
                        RaiseFailureEvents = true,
                        RaiseInformationEvents = true
                    }
                };

                coreApp.UseIdentityServer(options);
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            //app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            //{
            //    Authority = "https://localhost:44373/identity",

            //    ClientId = "mvc",
            //    Scope = "openid profile email roles rememberMe",
            //    ResponseType = "id_token",
            //    RedirectUri = "https://localhost:44367/",

            //    SignInAsAuthenticationType = "Cookies",
            //    UseTokenLifetime = false,

            //    Notifications = new OpenIdConnectAuthenticationNotifications
            //    {
            //        SecurityTokenValidated = async n =>
            //        {
            //            var id = n.AuthenticationTicket.Identity;

            //            var givenName = id.FindFirst(Constants.ClaimTypes.GivenName);
            //            var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName);
            //            var sub = id.FindFirst(Constants.ClaimTypes.Subject);
            //            var roles = id.FindFirst(Constants.ClaimTypes.Role);
            //            var rememberMe = id.FindFirst("rememberMe");

            //            var nid = new ClaimsIdentity(
            //                id.AuthenticationType,
            //                Constants.ClaimTypes.GivenName,
            //                Constants.ClaimTypes.Role);

            //            nid.AddClaim(givenName);
            //            nid.AddClaim(familyName);
            //            nid.AddClaim(sub);
            //            nid.AddClaim(roles);
            //            nid.AddClaim(rememberMe);

            //            nid.AddClaim(new Claim("app_specific", "some data"));

            //            n.AuthenticationTicket = new AuthenticationTicket(
            //               nid,
            //               n.AuthenticationTicket.Properties);

            //        }
            //    }
            //});

          //  app.UseResourceAuthorization(new AuthorizationManager());

            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = new Dictionary<string, string>();
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\identityServer\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }

    }
}