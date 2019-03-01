using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DAL.Entities;
using DAL.Configurations;

namespace DAL.EF
{
    public class TaskManagementContext : IdentityDbContext<ApplicationUser>
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<TaskInfo> TaskInfos { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonSetup());
            modelBuilder.ApplyConfiguration(new StatusSetup());
            modelBuilder.ApplyConfiguration(new TaskSetup());
            modelBuilder.ApplyConfiguration(new PrioritySetup());
            modelBuilder.ApplyConfiguration(new TeamSetup());

            base.OnModelCreating(modelBuilder);
        }
    }
}