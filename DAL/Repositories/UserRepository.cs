using DAL.Entities;
using DAL.Identity;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public ApplicationUserManager ApplicationUserManager { get; private set; }

        public UserRepository(ApplicationUserManager applicationUserManager)
        {
            ApplicationUserManager = applicationUserManager;
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await ApplicationUserManager.GetRolesAsync(user);
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            return await ApplicationUserManager.GetUsersInRoleAsync(roleName);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await ApplicationUserManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await ApplicationUserManager.CheckPasswordAsync(user, password);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await ApplicationUserManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await ApplicationUserManager.FindByNameAsync(userName);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await ApplicationUserManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return await ApplicationUserManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            return await ApplicationUserManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await ApplicationUserManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return await ApplicationUserManager.RemoveFromRoleAsync(user, role);
        }
    }
}
