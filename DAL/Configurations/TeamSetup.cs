using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;

namespace DAL.Configurations
{
    class TeamSetup : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            //builder
            //    .HasKey(e => e.Id);

            builder
                .Property(e => e.TeamName)
                .IsRequired()
                .HasMaxLength(60);

            builder
                .HasMany(e => e.People)
                .WithOne(e => e.Team)
                .HasForeignKey(e => e.TeamId);
        }
    }
}
