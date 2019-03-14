using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;

namespace DAL.Configurations
{
    public class ApplicationUserSetup : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .Property(e => e.FName)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(e => e.LName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
