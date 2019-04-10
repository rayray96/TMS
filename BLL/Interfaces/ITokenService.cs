using BLL.Exceptions;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    /// <summary>
    /// Service for working with tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Authentication of user.
        /// </summary>
        /// <param name="userName">Name of the current user</param>
        /// <param name="password">Password of the current user</param>
        /// ///<exception cref="RoleException">This user does not have a role</exception>
        /// ///<exception cref="InvalidPasswordException">This password is not valid</exception>
        /// ///<exception cref="UserNotFoundException">This user does not exist</exception>
        /// <returns>A claims-based identity</returns>
        Task<ClaimsIdentity> GetClaimsIdentityAsync(string userName, string password);

        /// <summary>
        /// Authentication of user.
        /// </summary>
        /// <param name="userId">Id of the current user</param>
        /// ///<exception cref="RoleException">This user does not have a role</exception>
        /// ///<exception cref="UserNotFoundException">This user does not exist</exception>
        /// <returns>A claims-based identity</returns>
        Task<ClaimsIdentity> GetClaimsIdentityAsync(string userId);

        /// <summary>
        /// Authentication of user.
        /// </summary>
        /// <param name="userId">Id of the current user</param>
        /// ///<exception cref="UserNotFoundException">This user does not exist</exception>
        /// <returns>New refresh token</returns>
        Task<RefreshTokenDTO> GenerateRefreshTokenAsync(string userName);

        /// <summary>
        /// Get refresh token from database.
        /// </summary>
        /// <param name="refreshToken">Id of the current user</param>
        /// ///<exception cref="RefreshTokenNotFoundException">This user does not exist</exception>
        /// <returns>Current refresh token from database</returns>
        RefreshTokenDTO GetRefreshToken(string refreshToken);

        /// <summary>
        /// Create the new access token.
        /// </summary>
        /// <param name="claims">Claims for generation token</param>
        /// <param name="minutesLife">Lifetime for new access token</param>
        /// <returns>New access token</returns>
        string GenerateToken(IEnumerable<Claim> claims, double minutesLife);

        /// <summary>
        /// Update the current refresh token.
        /// </summary>
        /// <param name="refreshToken">RefreshToken for updating</param>
        /// ///<exception cref="RefreshTokenNotFoundException">This user does not exist</exception>
        void UpdateRefreshToken(RefreshTokenDTO refreshToken);
    }
}
