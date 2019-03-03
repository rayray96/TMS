using DAL.Entities;
using DAL.EF;

namespace DAL.Repositories
{
    public class TeamRepository : Repository<Team>
    {
        public TeamRepository(ApplicationContext context)
            : base(context)
        {

        }
    }
}