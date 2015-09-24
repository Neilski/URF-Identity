using System;
using System.Data.Entity;
using Identity.Db.Configurations;
using Identity.Models;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Identity.Db
{
    public class UrfIdentityDbContext :
        IdentityDbContext
            <ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin,
                ApplicationUserRole, ApplicationUserClaim>
    {
        #region DataSets
        public DbSet<ApplicationUserDetails> UserDetails { get; set; }
        #endregion DataSets


        #region CTORs
        public UrfIdentityDbContext()
            : base("IdentityConnectionString")
        {
        }


        static UrfIdentityDbContext()
        {
            Database.SetInitializer<UrfIdentityDbContext>(null);
            // Database.SetInitializer
            // (new IdentityDbInitializer());
            // (new CreateDatabaseIfNotExists<IdentityDbContext>());
        }


        public static UrfIdentityDbContext Create()
        {
            return new UrfIdentityDbContext();
        }
        #endregion CTORs


        #region OnModelCreating
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new ApplicationUserDetailsMap());
        }
        #endregion OnModelCreating
    }
}