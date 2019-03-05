using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using DAL.Interfaces;
using DAL.EF;

namespace DAL.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public void Create(Person person, string teamName)
        {
            var team = new Team { TeamName = teamName };
            IEnumerable<Team> checkTeamExists = Context.Teams.Where(t => (t.TeamName == teamName));

            if (checkTeamExists.Any())
                throw new ArgumentException("This team already exists", teamName);

            Context.Teams.Add(team);
            person.TeamId = team.Id;
            Context.People.Add(person);
        }
    }
}
