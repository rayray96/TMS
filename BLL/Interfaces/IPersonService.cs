using BLL.DTO;
using BLL.Exceptions;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    /// <summary>
    /// Service for working with persons.
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Delete person from team.
        /// </summary>
        /// <param name="id">Id of person which need to delete from team</param>
        /// ///<exception cref="PersonNotFoundException">This person has not found</exception>
        void DeletePersonFromTeam(int id);

        /// <summary>
        /// Add persons to team of manager.
        /// </summary>
        /// <param name="persons">Array of Id of persons which needed to add to team of manager</param>
        /// <param name="managerId">Id of manager who want to add members to his team</param>
        /// ///<exception cref="PersonNotFoundException">No persons to adding or not all members was founded</exception>
        /// ///<exception cref="ManagerNotFoundException">This manager is unknown</exception>
        void AddPersonsToTeam(int[] persons, string managerId);

        /// <summary>
        /// Get enumeration of assignees for manager.
        /// </summary>
        /// <param name="managerId">String Id of current manager</param>
        /// <returns>Enumeration of person who can be assignee for this manager</returns>
        IEnumerable<PersonDTO> GetAssignees(string managerId);

        /// <summary>
        /// Get enumeration of assignees for manager.
        /// </summary>
        /// <param name="managerId">Int Id of current manager</param>
        /// <returns>Enumeration of person who can be assignee for this manager</returns>
        IEnumerable<PersonDTO> GetAssignees(int managerId);

        /// <summary>
        /// Get an enumeration of persons who are members of manager team.
        /// </summary>
        /// <param name="managerId">String Id of manager</param>
        /// <returns>Enumeration of persons who are members of manager team</returns>
        IEnumerable<PersonDTO> GetTeam(string managerId);

        /// <summary>
        /// Get an enumeration of persons who doesn't have a team.
        /// </summary>
        /// <returns>Enumerations of persons who doesn't have a team</returns>
        IEnumerable<PersonDTO> GetPeopleWithoutTeam();

        //IEnumerable<PersonDTO> GetPeopleInTeam(Person manager); // TODO: Rewrite!!!

        /// <summary>
        /// Get person with current Id.
        /// </summary>
        /// <param name="id">Int Id of needed person</param>
        /// <returns>Current person</returns>
        PersonDTO GetPerson(int id);

        /// <summary>
        /// Get person with current Id.
        /// </summary>
        /// <param name="id">String Id of needed person</param>
        /// <returns>Current person</returns>
        PersonDTO GetPerson(string id);
    }
}
