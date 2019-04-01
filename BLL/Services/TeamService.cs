using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BLL.Configurations;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;

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
        // TODO: Add to controller.
        public void ChangeTeamName(int teamId, string newName)
        {
            var team = db.Teams.GetById(teamId);

            if (team == null)
                throw new TeamNotFoundException("Invalid Team for changes");

            if (team.TeamName != newName)
            {
                db.Teams.Delete(teamId);
                team.TeamName = newName;
                db.Teams.Create(team);
            }
        }

        public void CreateTeam(PersonDTO manager, string teamName)
        {
            var team = new Team { TeamName = teamName };
            IEnumerable<Team> checkTeamExists = db.Teams.Find(t => (t.TeamName == teamName));

            if (checkTeamExists.Count() != 0)
                throw new TeamExistsException("This team already exists");

            if (manager.Role != "Manager")
                throw new ManagerNotFoundException("This user is not a manager");

            db.Teams.Create(team);
            manager.Team = mapper.Map<Team, TeamDTO>(team);
            db.People.Update(manager.Id, mapper.Map<PersonDTO, Person>(manager));

            db.Save();
        }
    }
}
