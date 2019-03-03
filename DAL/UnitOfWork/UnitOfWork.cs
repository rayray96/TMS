using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using DAL.Entities;
using DAL.Repositories;
using DAL.EF;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private PersonRepository personRepository;
        private StatusRepository statusRepository;
        private TaskRepository taskRepository;
        private PriorityRepository priorityRepository;
        private TeamRepository teamRepository;

        private ApplicationContext context;

        public UnitOfWork(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;

            context = new ApplicationContext(options);
        }

        public IRepository<Person> People
        {
            get
            {
                if (personRepository == null)
                    personRepository = new PersonRepository(context);

                return personRepository;
            }
        }

        public IRepository<Status> Statuses
        {
            get
            {
                if (statusRepository == null)
                    statusRepository = new StatusRepository(context);

                return statusRepository;
            }
        }

        public IRepository<TaskInfo> Tasks
        {
            get
            {
                if (taskRepository == null)
                    taskRepository = new TaskRepository(context);

                return taskRepository;
            }
        }

        public IRepository<Priority> Priorities
        {
            get
            {
                if (priorityRepository == null)
                    priorityRepository = new PriorityRepository(context);

                return priorityRepository;
            }
        }

        public IRepository<Team> Teams
        {
            get
            {
                if (teamRepository == null)
                    teamRepository = new TeamRepository(context);

                return teamRepository;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();

                    personRepository.Dispose();
                    teamRepository.Dispose();
                    statusRepository.Dispose();
                    priorityRepository.Dispose();
                    taskRepository.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
