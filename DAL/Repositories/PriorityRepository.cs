using DAL.Entities;
using DAL.EF;

namespace DAL.Repositories
{
    public class PriorityRepository : Repository<Priority>
    {
        public PriorityRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}