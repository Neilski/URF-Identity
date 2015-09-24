namespace Identity.Db.Configurations
{
    public class ApplicationUserDetailsMap 
        : EntityTypeConfiguration<ApplicationUserDetails>
    {

        public ApplicationUserDetailsMap()
        {

            ToTable("AspNetIdentity_UserDetails");

            // Primary Key
            HasKey(t => t.UserId);

            // Properties
            Property(t => t.FirstName).IsRequired().HasMaxLength(100).HasColumnName("FirstName");
            Property(t => t.LastName).IsRequired().HasMaxLength(100).HasColumnName("LastName");
            Property(t => t.JobTitle).IsRequired().HasMaxLength(100).HasColumnName("JobTitle");
            Property(t => t.Company).IsRequired().HasMaxLength(100).HasColumnName("Company");
            Property(t => t.Phone).IsRequired().HasMaxLength(100).HasColumnName("Phone");

            // Relationships
            this.HasRequired(e => e.User)
               .WithRequiredDependent(e => e.UserDetails);

        }

    }
}
