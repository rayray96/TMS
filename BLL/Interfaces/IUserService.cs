using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Infrastructure;

namespace BLL.Interfaces
{
    /// <summary>
    /// Service for working with users.
    /// </summary>
    public interface IUserService : IDisposable
    {
        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="userDTO">Model of the current user</param>
        /// <returns>Information about result of creating of the new user</returns>
        Task<IdentityOperation> CreateUserAsync(UserDTO userDTO);

        /// <summary>
        /// Update user role.
        /// </summary>
        /// <param name="userId">Id of the current user</param>
        /// <param name="roleName">New role for the current user</param>
        /// <returns>Information about result of updating a role for the current user</returns>
        Task<IdentityOperation> UpdateUserRoleAsync(string userId, string roleName);

        Task<IEnumerable<UserDTO>> GetAllWorkersAsync();

        Task<IEnumerable<UserDTO>> GetAllManagersAsync();

        Task<UserDTO> GetUserAsync(string userName);

        Task<UserDTO> GetUserByIdAsync(string userId);
    }
}
