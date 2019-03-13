using DAL.Entities;
using DAL.EF;

namespace DAL.Repositories
{
    public class PersonRepository : Repository<Person>
    {
        public PersonRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
