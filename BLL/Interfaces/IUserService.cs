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
        /// <returns>Information about result of creating</returns>
        Task<IdentityOperation> CreateUserAsync(UserDTO userDTO);

        Task<IdentityOperation> UpdateUserRole(string userId, string roleName);

        Task<IEnumerable<UserDTO>> GetAllWorkers();

        Task<IEnumerable<UserDTO>> GetAllManagers();

        Task<UserDTO> GetUser(string id);
    }
}
