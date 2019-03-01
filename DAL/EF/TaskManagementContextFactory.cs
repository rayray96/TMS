using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.EF
{
    public class TaskManagementContextFactory : IDesignTimeDbContextFactory<TaskManagementContext>
    {
        public TaskManagementContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskManagementContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=TMSDatabase; Trusted_Connection=True;");

            return new TaskManagementContext(optionsBuilder.Options);
        }
    }
}
