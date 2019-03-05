using DAL.Entities;
using DAL.EF;

namespace DAL.Repositories
{
    public class StatusRepository : Repository<Status>
    {
        public StatusRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}