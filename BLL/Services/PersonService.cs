using AutoMapper;
using BLL.Configurations;
using BLL.Configurations.FactoryMethod;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork db;
        private readonly IEmailService emailService;

        private IMapper mapper { get; set; }

        /// <summary>
        /// Dependency Injection to database repositories.
        /// </summary>
        /// <param name="uow"> Point to context of dataBase </param>
        public PersonService(IUnitOfWork uow, IEmailService emailService)
        {
            db = uow;
            this.emailService = emailService;

            // Using Factory Method.
            MapperCreator creator = new CommonCreator();
            IWrappedMapper wrappedMapper = creator.FactoryMethod();
            mapper = wrappedMapper.CreateMapping();
        }

        #region Methods which include work with team.

        public void DeletePersonFromTeam(int id)
        {
            Person person = db.People.GetById(id);

            if (person == null)
                throw new PersonNotFoundException($"This person \"{id}\" has not found");

            person.TeamId = null;

            IEnumerable<TaskInfo> tasks = db.Tasks.Find(t => t.AssigneeId == person.Id);
            foreach (var task in tasks)
            {
                task.Assignee = null;
                task.AssigneeId = task.AuthorId;
                db.Tasks.Update(task.Id, task);
            }

            db.People.Update(person.Id, person);
            db.Save();
        }

        public void AddPersonsToTeam(int[] persons, string managerId)
        {
            if ((persons == null) || (persons.Length == 0))
                throw new PersonNotFoundException("No persons to adding");

            Person manager = db.People.Find(m => (m.UserId == managerId)).SingleOrDefault();

            if (manager == null)
                throw new ManagerNotFoundException($"This manager \"{managerId}\" is unknown");

            IEnumerable<Person> newTeamMembers = db.People.Find(p => persons.Contains(p.Id));

            if (newTeamMembers.Count() != persons.Length)
                throw new PersonNotFoundException("Not all members was founded");

            foreach (var member in newTeamMembers)
            {
                member.TeamId = manager.TeamId;
                db.People.Update(member.Id, member);
            }

            var team = db.Teams.GetById(manager.TeamId.Value);

            db.Save();

            foreach (var member in newTeamMembers)
            {
                string emailBody = string.Format(EmailService.BODY_NEW_TEAM_MEMBER,
                                                 member.FName + " " + member.LName, team.TeamName, manager.FName + " " + manager.LName);
                emailService.Send(manager.Email, member.Email, EmailService.SUBJECT_NEW_TEAM_MEMBER, emailBody);
            }
        }

        public IEnumerable<PersonDTO> GetPeopleWithoutTeam()
        {
            var people = db.People.Find(p => (p.TeamId == null && p.Role == "Worker"));

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
                throw new ManagerNotFoundException("The argument manager is null");

            people = mapper.Map<IEnumerable<Person>, IEnumerable<PersonDTO>>(db.People.Find(p => ((p.Team != null) && (p.Team.Id == manager.Team.Id))));

            return people;
        }

        #endregion

        #region Methods which include work with person.

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

        public IEnumerable<PersonDTO> GetAssignees(int managerId)
        {
            var person = db.People.Find(p => p.Id == managerId).SingleOrDefault();
            var people = GetPeopleInTeam(person);

            return people;
        }

        public IEnumerable<PersonDTO> GetAssignees(string managerId)
        {
            var person = db.People.Find(p => p.UserId == managerId).SingleOrDefault();
            var people = GetPeopleInTeam(person);

            return people;
        }

        #endregion
    }
}
