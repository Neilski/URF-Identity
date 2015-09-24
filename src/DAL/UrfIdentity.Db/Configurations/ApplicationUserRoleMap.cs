using System.Data.Entity.ModelConfiguration;
using Identity.Models;


namespace UrfIdentity.Db.Configurations
{
    public class ApplicationUserRoleMap
        : EntityTypeConfiguration<ApplicationUserRole>
    {
        public ApplicationUserRoleMap()
        {
            ToTable("AspNetUserRoles");

            // Primary Key
            HasKey(t => new {t.UserId, t.RoleId});

            Property(t => t.UserId).IsRequired().HasColumnName("UserId");
            Property(t => t.RoleId).IsRequired().HasColumnName("RoleId");
        }
    }
}