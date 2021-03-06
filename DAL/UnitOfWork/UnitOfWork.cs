﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DAL.Interfaces;
using DAL.Entities;
using DAL.Repositories;
using DAL.EF;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository<Person> personRepository;
        private IRepository<Status> statusRepository;
        private IRepository<TaskInfo> taskRepository;
        private IRepository<Priority> priorityRepository;
        private IRepository<Team> teamRepository;

        private ApplicationDbContext Context { get; }
        private UserManager<ApplicationUser> UserManager { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
        }

        public IRepository<Person> People
        {
            get
            {
                if (personRepository == null)
                    personRepository = new PersonRepository(Context);

                return personRepository;
            }
        }

        public IRepository<Status> Statuses
        {
            get
            {
                if (statusRepository == null)
                    statusRepository = new StatusRepository(Context);

                return statusRepository;
            }
        }

        public IRepository<TaskInfo> Tasks
        {
            get
            {
                if (taskRepository == null)
                    taskRepository = new TaskRepository(Context);

                return taskRepository;
            }
        }

        public IRepository<Priority> Priorities
        {
            get
            {
                if (priorityRepository == null)
                    priorityRepository = new PriorityRepository(Context);

                return priorityRepository;
            }
        }

        public IRepository<Team> Teams
        {
            get
            {
                if (teamRepository == null)
                    teamRepository = new TeamRepository(Context);

                return teamRepository;
            }
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
