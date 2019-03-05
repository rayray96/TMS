using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;

namespace DAL.Configurations
{
    public class IdentityRoleSetup : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder
                .HasData(
                new IdentityRole[]
                {
                    new IdentityRole{ Name="Admin", NormalizedName="Admin".ToUpper() },
                    new IdentityRole{ Name="Manager", NormalizedName="Manager".ToUpper()},
                    new IdentityRole{ Name="Worker", NormalizedName="Worker".ToUpper()}
                }
                );
        }
    }
}
