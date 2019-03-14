using System;
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
    }
}
