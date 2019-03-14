using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DAL.Entities;

namespace DAL.Configurations
{
    public class TaskSetup : IEntityTypeConfiguration<TaskInfo>
    {
        public void Configure(EntityTypeBuilder<TaskInfo> builder)
        {
            builder
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(60);

            builder
                .HasOne(e => e.Priority)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.PriorityId);

            builder
                .HasOne(e => e.Assignee)
                .WithMany()
                .HasForeignKey(e => e.AssigneeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(e => e.Author)
                .WithMany()
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(e => e.Status)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.StatusId);
        }
    }
}
