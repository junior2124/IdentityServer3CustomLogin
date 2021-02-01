using System.Data.Entity;

namespace Id.Models
{
    public class ISSDBContext : DbContext
    {
        public ISSDBContext() : base("name=ConnectionString")
        {
            Database.SetInitializer<ISSDBContext>(new CreateDatabaseIfNotExists<ISSDBContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
    }
}