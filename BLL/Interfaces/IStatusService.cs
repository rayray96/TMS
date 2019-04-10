using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    /// <summary>
    /// Service for working with status of task.
    /// </summary>
    public interface IStatusService
    {
        /// <summary>
        /// Get all possible statuses for task.
        /// </summary>
        /// <returns>Enumeration of statuses</returns>
        IEnumerable<StatusDTO> GetAllStatuses();

        /// <summary>
        /// Get all statuses for assignee.
        /// </summary>
        /// <returns>Enumeration of statuses</returns>
        IEnumerable<StatusDTO> GetStatusesForAssignee();

        /// <summary>
        /// Get all statuses for author.
        /// </summary>
        /// <returns>Enumeration of statuses</returns>
        IEnumerable<StatusDTO> GetStatusesForAuthor();
    }
}
