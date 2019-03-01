using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;

namespace DAL.Configurations
{
    class PrioritySetup : IEntityTypeConfiguration<Priority>
    {
        public void Configure(EntityTypeBuilder<Priority> builder)
        {
            //builder
            //    .HasKey(e => e.Id);

            builder
                .Property(e => e.Name)
                .HasMaxLength(10)
                .IsRequired();

            builder
                .HasMany(e => e.Tasks)
                .WithOne(e => e.Priority)
                .HasForeignKey(e => e.PriorityId);

            builder
                .HasData(
                new Priority[]
                {
                    new Priority{ Id=1, Name="Low" },
                    new Priority{ Id=2, Name="Middle" },
                    new Priority{ Id=3, Name="High" }
                }
                );
        }
    }
}
