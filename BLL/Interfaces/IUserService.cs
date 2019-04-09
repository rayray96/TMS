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

        /// <summary>
        /// Get all users by the current role.
        /// </summary>
        /// <param name="roleName">Role name of the current user</param>
        /// <returns>All users in the current role</returns>
        Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName);

        /// <summary>
        /// Get user by the current name.
        /// </summary>
        /// <param name="userName">User name of the current user</param>
        /// <returns>User by the user name</returns>
        Task<UserDTO> GetUserByNameAsync(string userName);

        /// <summary>
        /// Get user by the current id.
        /// </summary>
        /// <param name="userId">Id of the current user</param>
        /// <returns>User by the user id</returns>
        Task<UserDTO> GetUserByIdAsync(string userId);
    }
}
