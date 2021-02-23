using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Id.EF
{
    public class Entities
    {
        public class Context : IdentityDbContext<IdentityUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
        {
            public Context(string connectionString) : base(connectionString) { }
        }

        public class UserStore : UserStore<IdentityUser, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
        {
            public UserStore(Context context) : base(context) { }
        }

        public class RoleStore : RoleStore<IdentityRole>
        {
            public RoleStore(Context context) : base(context) { }
        }


        public class UserManager : UserManager<IdentityUser, string>
        {
            public UserManager(UserStore userStore)
                : base(userStore)
            {
            }
        }

        public class RoleManager : RoleManager<IdentityRole>
        {
            public RoleManager(RoleStore roleStore) : base(roleStore) { }
        }
    }
}