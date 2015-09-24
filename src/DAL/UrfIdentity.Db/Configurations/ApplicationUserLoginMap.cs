using System.Data.Entity.ModelConfiguration;
using Identity.Models;


namespace UrfIdentity.Db.Configurations
{
    public class ApplicationUserLoginMap
        : EntityTypeConfiguration<ApplicationUserLogin>
    {
        public ApplicationUserLoginMap()
        {
            ToTable("AspNetUserLogins");

            // Primary Key
            HasKey(l => new {l.LoginProvider, l.ProviderKey, l.UserId});

            Property(l => l.UserId).IsRequired();
            Property(l => l.LoginProvider)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("LoginProvider");
            Property(l => l.ProviderKey)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("ProviderKey");
        }
    }
}