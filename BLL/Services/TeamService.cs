using System;
using System.Collections.Generic;
using AutoMapper;
using BLL.DTO;
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
        /// <param name="uow"> Point to context of DataBase </param>
        public TeamService(IUnitOfWork uow)
        {
            db = uow;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, PersonDTO>();
                cfg.CreateMap<Status, StatusDTO>();
                cfg.CreateMap<Priority, PriorityDTO>();
                cfg.CreateMap<Team, TeamDTO>();
                cfg.CreateMap<TaskInfo, TaskDTO>();
            });

            mapper = config.CreateMapper();
        }

        public void ChangeTeamName(int teamId, string NewName)
        {
            var team = db.Teams.GetById(teamId);

            if (team == null)
                throw new ArgumentException("Invalid Team for changes");

            if (team.TeamName != NewName)
            {
                db.Teams.Delete(teamId);
                team.TeamName = NewName;
                db.Teams.Create(team);
            }
        }

        public IEnumerable<TeamDTO> GetAllTeams()
        {
            var result = db.Teams.GetAll();

            return mapper.Map<IEnumerable<Team>, IEnumerable<TeamDTO>>(result);
        }

        public void CreateTeam(Person manager, string teamName)
        {
            var team = new Team { TeamName = teamName };
            IEnumerable<Team> checkTeamExists = db.Teams.Find(t => (t.TeamName == teamName));

            if (checkTeamExists != null)
                throw new ArgumentException("This team already exists", teamName);

            if (manager.Role != "Manager")
                throw new ArgumentException("This user is not a manager", manager.Name);

            db.Teams.Create(team);
            manager.TeamId = team.Id;
            db.Save();
        }
    }
}
