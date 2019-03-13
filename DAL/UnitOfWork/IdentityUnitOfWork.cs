using System;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Entities;
using DAL.Repositories;
using DAL.EF;
using DAL.Identity;

namespace DAL.UnitOfWork
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private IUserRepository userRepository;
        private IRepository<RefreshToken> refreshTokenRepository;
        private IRepository<Person> personRepository;

        private ApplicationDbContext Context { get; }
        private ApplicationUserManager ApplicationUser { get; }

        public IdentityUnitOfWork(ApplicationDbContext context, ApplicationUserManager applicationUser)
        {
            Context = context;
            ApplicationUser = applicationUser;
        }

        public IUserRepository Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(ApplicationUser);

                return userRepository;
            }
        }

        public IRepository<RefreshToken> RefreshTokens
        {
            get
            {
                if (refreshTokenRepository == null)
                    refreshTokenRepository = new RefreshTokenRepository(Context);

                return refreshTokenRepository;
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
