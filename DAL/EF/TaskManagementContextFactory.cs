using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.EF
{
    public class TaskManagementContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=TMSDatabase; Trusted_Connection=True;");

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
