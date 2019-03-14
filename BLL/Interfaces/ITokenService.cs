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
        /// <returns>A claims-based identity</returns>
        Task<ClaimsIdentity> GetClaimsIdentityAsync(string userName, string password);
        
        Task<RefreshTokenDTO> GenerateRefreshTokenAsync(string userName);
        
        Task<ClaimsIdentity> GetClaimsIdentityAsync(string userId);

        RefreshTokenDTO GetRefreshToken(string refreshToken);

        string GenerateToken(IEnumerable<Claim> claims, double minutesLife);

        void UpdateRefreshToken(RefreshTokenDTO refreshToken);
    }
}
