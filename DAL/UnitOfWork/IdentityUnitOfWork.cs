using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using DAL.Entities;
using DAL.Repositories;
using DAL.EF;

namespace DAL.UnitOfWork
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private UserRepository userRepository;
        private PersonRepository personRepository;

        private ApplicationDbContext Context { get; }
        private UserManager<ApplicationUser> UserManager { get; }

        public IdentityUnitOfWork(string connectionString, UserManager<ApplicationUser> userManager)
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionBuilder.UseSqlServer(connectionString);

            Context = new ApplicationDbContext(optionBuilder.Options);
            this.UserManager = userManager;
        }

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(UserManager);

                return userRepository;
            }
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

        public void Save()
        {
            Context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                    userRepository.Dispose();
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
