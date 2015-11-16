using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using UrfIdentity.DAL.Db.Configurations;
using UrfIdentity.DAL.Db.Validators;
using UrfIdentity.Models;


namespace UrfIdentity.DAL.Db
{
    public class UrfIdentityDataContext
        : DataContext, IUrfIdentityDataContextAsync
    {
        #region DataSets
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ApplicationUserClaim> ApplicationUserClaims { get; set; }
        public DbSet<ApplicationUserLogin> ApplicationUserLogins { get; set; }
        public DbSet<ApplicationUserDetails> ApplicationUserDetails { get; set; }
        #endregion DataSets



        #region Constructors
        public UrfIdentityDataContext()
            : base("Name=DefaultConnectionString")
        {
            // Default URF configuration
            // Configuration.LazyLoadingEnabled = false;
            // Configuration.ProxyCreationEnabled = false;

            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }


        public static UrfIdentityDataContext Create()
        {
            return new UrfIdentityDataContext();
        }


        static UrfIdentityDataContext()
        {
            // Database.SetInitializer<UrfIdentityDataContext>(null);

            Database.SetInitializer
                (new CreateDatabaseIfNotExists<UrfIdentityDataContext>());
        }
        #endregion Constructors



        #region OnModelCreating
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ASP.NET User Identity
            modelBuilder.Configurations.Add(new ApplicationRoleMap());
            modelBuilder.Configurations.Add(new ApplicationUserMap());
            modelBuilder.Configurations.Add(new ApplicationUserClaimMap());
            modelBuilder.Configurations.Add(new ApplicationUserLoginMap());
            modelBuilder.Configurations.Add(new ApplicationUserRoleMap());
            modelBuilder.Configurations.Add(new ApplicationUserDetailsMap());
        }
        #endregion OnModelCreating



        #region URF Override
        /// <summary>
        /// Set the entity state to reflect the URF IObjectState.
        /// </summary>
        /// <param name="dbEntityEntry"></param>]
        /// <remarks>
        /// This virtual method was introduced into the core URF library as a sub-method
        /// DataContext's ProcessObjectStatePreCommit() method.  This provided the
        /// minimum change to the URF source code.
        /// 
        /// Neil Martin - 16th November 2015
        /// </remarks>
        protected override void ProcessObjectStatePreCommit(DbEntityEntry dbEntityEntry)
        {
            var baseType = ObjectContext.GetObjectType(dbEntityEntry.Entity.GetType());

            if (
                String.Compare(baseType.AssemblyQualifiedName,
                    typeof (ApplicationUser).AssemblyQualifiedName,
                    StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                base.ProcessObjectStatePreCommit(dbEntityEntry);
                return;
            }


            // Process Identity entities differently as the Identity 2.0 framework knows 
            // nothing about IObjectState and will not set it correctly, so...
            //
            // Check the base entity state against the URF IObjectState state to see
            // if the update has been managed by the URF framework (as opposed to the
            // Microsoft Identity 2.0 framework), otherwise just leave the base entity
            // state as it is.
            var urfEntityState =
                StateHelper.ConvertState(
                    ((IObjectState) dbEntityEntry.Entity).ObjectState);

            var entityState = dbEntityEntry.State;

            if ((entityState == EntityState.Unchanged) &&
                (urfEntityState != EntityState.Unchanged))
            {
                dbEntityEntry.State = urfEntityState;
            }
        }
        #endregion URF Override



        #region Validation
        // See http://msdn.microsoft.com/en-gb/data/gg193959.aspx
        // and http://stackoverflow.com/a/18736484/236860

        protected override DbEntityValidationResult
            ValidateEntity(
            DbEntityEntry entityEntry,
            IDictionary<object, object> items)
        {
            // Base validation for Data Annotations, IValidatableObject
            var result = base.ValidateEntity(entityEntry, items);

            // Only validate entities new/updated entities
            if ((result.Entry.State != EntityState.Added) &&
                (result.Entry.State != EntityState.Modified))
            {
                return result;
            }

            // Validate User Identity
            this.ValidateApplicationUser(result);
            this.ValidateApplicationRole(result);

            return result;
        }
        #endregion Validation
    }
}