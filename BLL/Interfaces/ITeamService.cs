using BLL.DTO;
using BLL.Exceptions;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    /// <summary>
    /// Service for working with teams.
    /// </summary>
    public interface ITeamService
    {
        /// <summary>
        /// Get all teams that are contained in database.
        /// </summary>
        /// <returns>Enumeration of teams in type TeamBLL</returns>
        IEnumerable<TeamDTO> GetAllTeams();

        /// <summary>
        /// Create a new team.
        /// </summary>
        /// <param name="id">Id of the current team</param>
        /// <exception cref="TeamExistsException">Manager has a team or team already exists</exception>
        /// <exception cref="ManagerNotFoundException">Manager has not found</exception>
        void CreateTeam(PersonDTO manager, string teamName);

        /// <summary>
        /// Get team name by team id.
        /// </summary>
        /// <param name="id">Id of the current team</param>
        /// <exception cref="TeamNotFoundException">Team has not found</exception>
        string GetTeamNameById(int id);

        /// <summary>
        /// Change name of team.
        /// </summary>
        /// <param name="id">Id of the currentteam</param>
        /// <param name="NewName">New name for team</param>
        /// <exception cref="TeamNotFoundException">Team has not found</exception>
        void ChangeTeamName(int id, string newName);       
    }
}
