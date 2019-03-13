using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Infrastructure;
using Microsoft.AspNetCore.Identity;

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
        Task<IdentityOperation> CreateAsync(UserDTO userDTO);

        /// <summary>
        /// Authentication of user.
        /// </summary>
        /// <param name="userName">Name of the current user</param>
        /// <param name="password">Password of the current user</param>
        /// <returns>A claims-based identity</returns>
        Task<ClaimsIdentity> GetClaimsIdentityAsync(string userName, string password);

        Task<IdentityOperation> SetRefreshToken(string userName, string refreshToken);
        string GenerateToken(IEnumerable<Claim> claims, int minutesLife);
        RefreshTokenDTO GetRefreshToken(string refreshToken);
        Task<ClaimsIdentity> GetClaimsIdentityAsync(string userId);
        void UpdateToken(RefreshTokenDTO refreshToken);
    }
}
