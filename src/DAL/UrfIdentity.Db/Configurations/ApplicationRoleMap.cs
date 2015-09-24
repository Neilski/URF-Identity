using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Identity.Models;


namespace UrfIdentity.Db.Configurations
{
    public class ApplicationRoleMap
        : EntityTypeConfiguration<ApplicationRole>
    {
        public ApplicationRoleMap()
        {
            ToTable("AspNetRoles");

            // Primary Key
            HasKey(r => r.Id);

            Property(r => r.Id).IsRequired().HasColumnName("Id");

            Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("Name")
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("RoleNameIndex")
                    {
                        IsUnique = true
                    }));

            // Navigation
            HasMany(r => r.Users)
                .WithRequired()
                .HasForeignKey(ur => ur.RoleId)
                .WillCascadeOnDelete();
        }
    }
}