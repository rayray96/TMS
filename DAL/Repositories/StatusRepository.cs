using DAL.Entities;
using DAL.EF;

namespace DAL.Repositories
{
    public class StatusRepository : Repository<Status>
    {
        public StatusRepository(ApplicationContext context)
            : base(context)
        {

        }
    }
}