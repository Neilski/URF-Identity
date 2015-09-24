using System.Data.Entity.ModelConfiguration;
using UrfIdentity.Models;


namespace UrfIdentity.DAL.Db.Configurations
{
    public class ApplicationUserDetailsMap
        : EntityTypeConfiguration<ApplicationUserDetails>
    {
        public ApplicationUserDetailsMap()
        {
            ToTable("AspNetUserDetails");

            // Primary Key
            HasKey(d => d.UserId);

            // Properties
            Property(d => d.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("FirstName");
            Property(d => d.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("LastName");
            Property(d => d.JobTitle)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("JobTitle");
            Property(d => d.Company)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Company");
            Property(d => d.Phone)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Phone");
        }
    }
}