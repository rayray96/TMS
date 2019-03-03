using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IRoleRepository: IDisposable
    {
        Task<ApplicationRole> FindByNameAsync(string roleName);
        Task<ApplicationRole> FindByIdAsync(string roleId);

        Task<IdentityResult> CreateAsync(ApplicationRole role);
        Task<IdentityResult> DeleteAsync(ApplicationRole role);
        Task<bool> RoleExistsAsync(string roleName);
    }
}
