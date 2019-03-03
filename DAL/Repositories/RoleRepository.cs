using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DAL.Interfaces;
using DAL.Entities;
using DAL.EF;

namespace DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public RoleManager<ApplicationRole> RoleManager { get; private set; }

        public RoleRepository(RoleManager<ApplicationRole> roleManager)
        {
            RoleManager = roleManager;
        }

        public async Task<ApplicationRole> FindByNameAsync(string roleName)
        {
            return await RoleManager.FindByNameAsync(roleName);
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId)
        {
            return await RoleManager.FindByIdAsync(roleId);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role)
        {
            return await RoleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role)
        {
            return await RoleManager.DeleteAsync(role);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await RoleManager.RoleExistsAsync(roleName);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    RoleManager.Dispose();
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