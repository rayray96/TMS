using AutoMapper;
using BLL.Configurations;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork db;
        private IMapper mapper { get; set; }

        /// <summary>
        /// Dependency Injection to database repositories.
        /// </summary>
        /// <param name="uow"> Point to context of dataBase </param>
        public TeamService(IUnitOfWork uow)
        {
            db = uow;
            mapper = MapperConfig.GetMapperResult();
        }

        public IEnumerable<TeamDTO> GetAllTeams()
        {
            var result = db.Teams.GetAll();

            return mapper.Map<IEnumerable<Team>, IEnumerable<TeamDTO>>(result);
        }

        public string GetTeamNameById(int id)
        {
            var team = db.Teams.GetById(id);

            if (team == null)
                throw new TeamNotFoundException("Team with current id does not exist");

            return team.TeamName;
        }

        public void ChangeTeamName(int id, string newName)
        {
            var team = db.Teams.GetById(id);

            if (team == null)
                throw new TeamNotFoundException("Invalid Team Id for changes");

            if (team.TeamName != newName)
            {
                team.TeamName = newName;
                db.Teams.Update(id, team);
                db.Save();
            }
        }

        public void CreateTeam(PersonDTO manager, string teamName)
        {
            if (manager.TeamId != null)
                throw new TeamExistsException("This manager has got a team");

            var team = new Team { TeamName = teamName };
            IEnumerable<Team> checkTeamExists = db.Teams.Find(t => (t.TeamName == teamName));

            if (checkTeamExists.Count() != 0)
                throw new TeamExistsException("This team already exists");

            if (manager.Role != "Manager")
                throw new ManagerNotFoundException("This user is not a manager");

            db.Teams.Create(team);
            manager.TeamId = team.Id;
            db.People.Update(manager.Id, mapper.Map<PersonDTO, Person>(manager));

            db.Save();
        }
    }
}
