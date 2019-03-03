using System;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.EF;
using DAL.Repositories;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace DAL.UnitOfWork
{
    class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        //private readonly ApplicationContext context;

        public IUserRepository UserRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }

        public IdentityUnitOfWork(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            //if (context == null)
            //    throw new ArgumentNullException(nameof(context));

            UserRepository = userRepository ?? throw new ArgumentNullException();
            RoleRepository = roleRepository ?? throw new ArgumentNullException();
        }

        //public async Task SaveAsync()
        //{
        //    await context.SaveChangesAsync();
        //}

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UserRepository.Dispose();
                    RoleRepository.Dispose();

                    //context.Dispose();
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
