using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(IdentityClient.Startup))]

namespace IdentityClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44373/core",
                ClientId = "mvc",
                Scope = "openid profile roles",
                RedirectUri = "https://localhost:44367/",
                ResponseType = "id_token",

                SignInAsAuthenticationType = "Cookies"
            });
        }
    }
}