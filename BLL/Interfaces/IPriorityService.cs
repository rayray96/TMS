using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    /// <summary>
    /// Service for working with priorities of tasks.
    /// </summary>
    public interface IPriorityService
    {
        /// <summary>
        /// Get all possible tasks of priorities sets.
        /// </summary>
        /// <returns>Enumerations of tasks</returns>
        IEnumerable<PriorityDTO> GetAllPriorities();

        /// <summary>
        /// Get all tasks with current priority.  
        /// </summary>
        /// <param name="priorityName">Name of current priority</param>
        /// <returns>Enumerations of tasks</returns>
        IEnumerable<TaskDTO> GetTaskWithPriority(int id);
    }
}
