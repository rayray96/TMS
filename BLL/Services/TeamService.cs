using System;
using System.Collections.Generic;
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

        public void CreateTeam(Person manager, string teamName)
        {
            var team = new Team { TeamName = teamName };
            IEnumerable<Team> checkTeamExists = db.Teams.Find(t => (t.TeamName == teamName));

            if (checkTeamExists != null)
                throw new ArgumentException("This team already exists", teamName);

            if (manager.Role != "Manager")
                throw new ArgumentException("This user is not a manager", manager.UserName);

            db.Teams.Create(team);
            manager.TeamId = team.Id;
            db.People.Create(manager);

            db.Save();
        }
    }
}
