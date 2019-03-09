using DAL.Entities;
using DAL.EF;
using DAL.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DAL.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public void CreateTeam(Person manager, string teamName)
        {
            var team = new Team { TeamName = teamName };
            IEnumerable<Team> checkTeamExists = Context.Teams.Where(t => (t.TeamName == teamName));

            if (checkTeamExists != null)
                throw new ArgumentException("This team already exists", teamName);

            if (manager.Role != "Manager")
                throw new ArgumentException("This user is not a manager", manager.Name);

            Context.Teams.Add(team);
            manager.TeamId = team.Id;
            Context.People.Add(manager);
            //Context.SaveChanges();
        }
    }
}
