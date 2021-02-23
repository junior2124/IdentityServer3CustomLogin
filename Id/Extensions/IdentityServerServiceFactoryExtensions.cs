using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.EntityFramework;
using Id.IdentityServer;
using Id.Models;
using Id.EF;
using System.Collections.Generic;
using System.Linq;

namespace Id.Extensions
{
    public static class IdentityServerServiceFactoryExtensions
    {
        public static IdentityServerServiceFactory Configure(this IdentityServerServiceFactory factory, string connectionString)
        {
            var efConfig = new EntityFrameworkServiceOptions
            {
                ConnectionString = connectionString,
                //SynchronousReads = true
            };

            // these two calls just pre-populate the test DB from the in-memory config
            ConfigureClients(Clients.Get(), efConfig);
            ConfigureScopes(Scopes.Get(), efConfig);
            ConfigureUsers(Users.Get(), efConfig);

            var serviceOptions = new EntityFrameworkServiceOptions { ConnectionString = connectionString };
            factory.RegisterOperationalServices(serviceOptions);
            factory.RegisterConfigurationServices(serviceOptions);

            factory.Register(new Registration<Entities.Context>(resolver => new Entities.Context(connectionString)));
            factory.Register(new Registration<Entities.UserStore>());
            factory.Register(new Registration<Entities.UserManager>());

            var userService = new EulaAtLoginUserService();
            factory.UserService = new Registration<IUserService>(resolver => userService);

            factory.ViewService = new Registration<IViewService, CustomViewService>();

            return factory;
        }

        public static void ConfigureClients(IEnumerable<Client> clients, EntityFrameworkServiceOptions options)
        {
            using (var db = new ClientConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Clients.Any())
                {
                    foreach (var c in clients)
                    {
                        var e = c.ToEntity();
                        db.Clients.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }

        public static void ConfigureScopes(IEnumerable<Scope> scopes, EntityFrameworkServiceOptions options)
        {
            using (var db = new ScopeConfigurationDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Scopes.Any())
                {
                    foreach (var s in scopes)
                    {
                        var e = s.ToEntity();
                        db.Scopes.Add(e);
                    }
                    db.SaveChanges();
                }
            }
        }

        public static void ConfigureUsers(IEnumerable<User> users, EntityFrameworkServiceOptions options)
        {
            using (var db = new BaseDbContext(options.ConnectionString, options.Schema))
            {
                if (!db.Database.Exists())
                {
                    foreach (var s in users)
                    {
                        //var e = s.ToEntity();
                        //db.Scopes.Add(e);
                    }
                    //db.SaveChanges();
                }
            }
        }
    }
}