using System;
using System.Security.Claims;
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
        Task<IdentityOperation> Create(UserDTO userDTO);

        /// <summary>
        /// Authentication of user.
        /// </summary>
        /// <param name="userDTO">Model of the current user</param>
        /// <returns>A claims-based identity</returns>
        Task<ClaimsIdentity> Authenticate(UserDTO userDTO);
    }
}
