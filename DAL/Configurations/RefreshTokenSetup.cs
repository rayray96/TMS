using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class RefreshTokenSetup : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder
                 .HasKey(e => e.Id);

            builder
                 .Property(e => e.Token)
                 .IsRequired();

            builder
                .HasOne(e => e.ApplicationUser)
                .WithMany(e => e.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}
