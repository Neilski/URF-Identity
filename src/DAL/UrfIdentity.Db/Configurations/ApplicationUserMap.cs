using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Identity.Models;


namespace UrfIdentity.Db.Configurations
{
    public class ApplicationUserMap
        : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserMap()
        {
            ToTable("AspNetUsers");

            // Primary Key
            HasKey(t => t.Id);

            Property(u => u.Id).IsRequired().HasColumnName("Id");
            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("UserName")
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UserNameIndex")
                    {
                        IsUnique = true
                    }));
            Property(u => u.Email).IsRequired().HasMaxLength(256).HasColumnName("Email");
            Property(u => u.EmailConfirmed).IsRequired().HasColumnName("EmailConfirmed");
            Property(u => u.PasswordHash).HasColumnName("PasswordHash");
            Property(u => u.SecurityStamp).HasColumnName("SecurityStamp");
            Property(u => u.PhoneNumber).HasColumnName("PhoneNumber");
            Property(u => u.PhoneNumberConfirmed)
                .IsRequired()
                .HasColumnName("PhoneNumberConfirmed");
            Property(u => u.TwoFactorEnabled)
                .IsRequired()
                .HasColumnName("TwoFactorEnabled");
            Property(u => u.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc");
            Property(u => u.LockoutEnabled).IsRequired().HasColumnName("LockoutEnabled");
            Property(u => u.AccessFailedCount)
                .IsRequired()
                .HasColumnName("AccessFailedCount");
            Property(u => u.Created).IsRequired().HasColumnName("Created");
            Property(u => u.LastLogin).HasColumnName("LastLogin");


            // Relationships
            HasRequired(u => u.UserDetails)
                .WithRequiredPrincipal(u => u.User)
                .WillCascadeOnDelete();
            HasMany(u => u.Roles)
                .WithRequired()
                .HasForeignKey(ur => ur.UserId)
                .WillCascadeOnDelete();
            HasMany(u => u.Claims)
                .WithRequired()
                .HasForeignKey(uc => uc.UserId)
                .WillCascadeOnDelete();
            HasMany(u => u.Logins)
                .WithRequired()
                .HasForeignKey(ul => ul.UserId)
                .WillCascadeOnDelete();
        }
    }
}