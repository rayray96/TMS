using DAL.Entities;
using DAL.EF;

namespace DAL.Repositories
{
    public class TaskRepository : Repository<TaskInfo>
    {
        public TaskRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}