using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;

namespace DAL.Configurations
{
    class StatusSetup : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(60);

            builder
                .HasMany(e => e.Tasks)
                .WithOne(e => e.Status)
                .HasForeignKey(e => e.StatusId);

            builder
                .HasData(
                new Status[]
                {
                    new Status{ Id=1, Name="Not Started" },
                    new Status{ Id=2, Name="In Progress"},
                    new Status{ Id=3, Name="Test"},
                    new Status{ Id=4, Name="Almost Ready"},
                    new Status{ Id=5, Name="Executed"},
                    new Status{ Id=6, Name="Completed"},
                    new Status{ Id=7, Name="Canceled"}
                }
                );
        }
    }
}
