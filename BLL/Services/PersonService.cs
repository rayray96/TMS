using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;

namespace BLL.Services
{
    public class PersonService : IPersonService // Finish with services!
    {
        private readonly IUnitOfWork db;
        private readonly IEmailService emailService;

        private IMapper mapper { get; set; }

        /// <summary>
        /// Dependency Injection to database repositories.
        /// </summary>
        /// <param name="uow"> Point to context of DataBase </param>
        public PersonService(IUnitOfWork uow, IEmailService emailService)
        {
            db = uow;
            this.emailService = emailService;

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

        #region Team

        public void DeletePersonFromTeam(int id)
        {
            Person person = db.People.GetById(id);

            if (person == null)
                throw new ArgumentException("Person is not exist");

            person.TeamId = null;
            db.People.Update(person);
            db.Save();
        }

        public void AddPersonsToTeam(int[] persons, string managerId)
        {
            if ((persons == null) || (persons.Length == 0))
                throw new ArgumentException("No persons to adding", "persons");

            if (string.IsNullOrWhiteSpace(managerId))
                throw new ArgumentException("Unknown manager", "managerId");

            Person manager = db.People.Find(m => (m.UserId == managerId)).SingleOrDefault();

            if (manager == null)
                throw new ArgumentException("Unknown manager", "managerId");

            IEnumerable<Person> newTeamMembers = db.People.Find(p => persons.Contains(p.Id));

            if (newTeamMembers.Count() != persons.Length)
                throw new ArgumentException("Not all members was founded");

            foreach (var member in newTeamMembers)
            {
                member.TeamId = manager.TeamId;
                db.People.Update(member);
            }

            db.Save();

            foreach (var member in newTeamMembers)
            {
                string emailBody = string.Format(EmailService.BODY_NEW_TEAM_MEMBER,
                                                 member.Name, manager.Team.TeamName, manager.Name);
                emailService.Send(manager.Email, member.Email, EmailService.SUBJECT_NEW_TEAM_MEMBER, emailBody);
            }
        }

        public IEnumerable<PersonDTO> GetPeopleWithoutTeam()
        {
            var people = db.People.Find(p => (p.TeamId == null));

            return mapper.Map<IEnumerable<Person>, IEnumerable<PersonDTO>>(people);
        }

        public IEnumerable<PersonDTO> GetTeam(string managerId)
        {
            var manager = db.People.Find(p => (p.UserId == managerId)).SingleOrDefault();
            var people = GetPeopleInTeam(manager).Where(p => (p.Id != manager.Id));

            return people;
        }

        private IEnumerable<PersonDTO> GetPeopleInTeam(Person manager)
        {
            IEnumerable<PersonDTO> people = new List<PersonDTO>();
            if (manager == null)
                throw new ArgumentException("Manager is not found", "managerId");

            if (manager.Team == null)
                return people;

            people = mapper.Map<IEnumerable<Person>, IEnumerable<PersonDTO>>(db.People.Find(p => ((p.Team != null) && (p.Team.Id == manager.Team.Id))));

            return people;
        }

        #endregion

        #region GetPerson

        public PersonDTO GetPerson(int id)
        {
            var person = db.People.GetById(id);

            return mapper.Map<Person, PersonDTO>(person);
        }

        public PersonDTO GetPerson(string id)
        {
            var person = db.People.Find(p => p.UserId == id).SingleOrDefault();

            return mapper.Map<Person, PersonDTO>(person);
        }

        #endregion

        #region GetAssignee

        public IEnumerable<PersonDTO> GetAssignees(string managerId)
        {
            var manager = db.People.Find(p => p.UserId == managerId).SingleOrDefault();
            var people = GetPeopleInTeam(manager);

            return people;
        }

        public IEnumerable<PersonDTO> GetAssignees(int managerId)
        {
            var manager = db.People.Find(p => p.Id == managerId).SingleOrDefault();
            var people = GetPeopleInTeam(manager);

            return people;
        }

        #endregion
    }
}
