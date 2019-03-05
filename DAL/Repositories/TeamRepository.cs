using DAL.Entities;
using DAL.EF;

namespace DAL.Repositories
{
    public class TeamRepository : Repository<Team>
    {
        public TeamRepository(ApplicationDbContext context)
            : base(context)
        {

        }
    }
}