using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DAL.Interfaces;
using DAL.Entities;
using DAL.Repositories;
using DAL.EF;

namespace DAL.UnitOfWork
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private UserRepository userRepository;
        private RoleRepository roleRepository;
        private PersonRepository personRepository;

        private ApplicationDbContext Context { get; }
        private UserManager<ApplicationUser> UserManager { get; }
        private RoleManager<ApplicationRole> RoleManager { get; }

        public IdentityUnitOfWork(UserManager<ApplicationUser> userManager)
        {
            Context = new ApplicationDbContext();
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

        public IRoleRepository Roles
        {
            get
            {
                if (roleRepository == null)
                    roleRepository = new RoleRepository(RoleManager);

                return roleRepository;
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
