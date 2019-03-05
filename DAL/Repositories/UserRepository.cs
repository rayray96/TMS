using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DAL.Interfaces;
using DAL.Entities;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserManager<ApplicationUser> UserManager { get; private set; }

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await UserManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> FindByLoginAsync(string userName, string password)
        {
            return await UserManager.FindByLoginAsync(userName, password);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await UserManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await UserManager.FindByNameAsync(userName);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            return await UserManager.CreateAsync(user);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return await UserManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            return await UserManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await UserManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return await UserManager.RemoveFromRoleAsync(user, role);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UserManager.Dispose();
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
