using System.Data.Entity.ModelConfiguration;
using UrfIdentity.Models;


namespace UrfIdentity.DAL.Db.Configurations
{
    public class ApplicationUserClaimMap
        : EntityTypeConfiguration<ApplicationUserClaim>
    {
        public ApplicationUserClaimMap()
        {
            ToTable("AspNetUserClaims");

            // Primary Key
            HasKey(c => new {c.Id, c.UserId});

            Property(c => c.Id).IsRequired().HasColumnName("Id");
            Property(c => c.UserId).IsRequired().HasColumnName("UserId");
            Property(c => c.ClaimType).HasColumnName("ClaimType");
            Property(c => c.ClaimValue).HasColumnName("ClaimValue");
        }
    }
}