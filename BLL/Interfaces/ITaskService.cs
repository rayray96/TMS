using System;
using System.Collections.Generic;
using BLL.DTO;

namespace BLL.Interfaces
{
    /// <summary>
    /// Service for working with tasks.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Get all possible templates of priorities sets.
        /// </summary>
        /// <returns>Enumerations of templates</returns>
        IEnumerable<PriorityDTO> GetAllPriorities();

        /// <summary>
        /// Delete task.
        /// </summary>
        /// <param name="taskId">Id of task for deleting</param>
        /// <param name="currentUserName">Name of person who try to delete this task</param>
        /// ///<exception cref="ArgumentException">Task with this Id wasn't found</exception>
        /// ///<exception cref="InvalidOperationException">If person who want to delete task isn't its author</exception>
        void DeleteTask(int taskId, string currentUserName);

        /// <summary>
        /// Add task to database.
        /// </summary>
        /// <param name="task">Task to adding</param>
        /// <param name="authorName">Name of tasks author</param>
        /// <param name="assigneeName">Name of tasks assignee if null -  assignee will be an author</param>
        /// ///<exception cref="ArgumentNullException">Name of author is null or empty</exception>
        /// ///<exception cref="Exception">Problem with status new in database. </exception>  
        void CreateTask(TaskDTO task, string authorName, string assigneeName);
        
        /// <summary>
        /// Save task which was edit by user.
        /// </summary>
        /// <param name="task">Task with edits</param>
        /// <param name="assigneeName">Name of assignee of task</param>
        /// ///<exception cref="ArgumentNullException">Name of assignee is null or empty</exception>  
        void SaveChangeTask(TaskDTO task, string assigneeName);

        /// <summary>
        /// Set new status to task.
        /// </summary>
        /// <param name="taskid">Id of task that need to set new status</param>
        /// <param name="statusName">Name of new status for task</param>
        /// ///<exception cref="ArgumentNullException">Name of status is null or empty</exception>
        /// ///<exception cref="ArgumentException">task for edit wasn't found</exception>   
        void SetNewStatus(int taskid, string statusName);

        /// <summary>
        /// Get task with Id.
        /// </summary>
        /// <param name="id">Id of task</param>
        /// <returns>Task</returns>
        TaskDTO GetTask(int id);

        /// <summary>
        /// Get tasks of team of Manager with shown Id.
        /// </summary>
        /// <param name="managerId">Id of manager of team</param>
        /// ///<exception cref="ArgumentException">Manager with this Id wasn't found</exception>   
        /// <returns>Enumeration of tasks</returns>
        IEnumerable<TaskDTO> GetTasksOfTeam(string managerId);

        /// <summary>
        /// Get tasks of team where Due date is in past
        /// </summary>
        /// <param name="teamId">Id of team</param>
        /// <returns>Enumeration of tasks</returns>
        IEnumerable<TaskDTO> GetOverDueTasks(int teamId);

        /// <summary>
        /// Get tasks of team where status is Closed
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        IEnumerable<TaskDTO> GetCompleteTasks(int teamId);

        /// <summary>
        /// Get all tasks without subtasks of shown assignee  
        /// </summary>
        /// <param name="id">string Id of assignee</param>
        /// <returns>Enumerations of tasks</returns>
        IEnumerable<TaskDTO> GetTaskOfAssignee(string id);

        /// <summary>
        /// Get all tasks without subtasks of shown assignee  
        /// </summary>
        /// <param name="id">string Id of assignee</param>
        /// <returns>Enumerations of tasks</returns>
        IEnumerable<TaskDTO> GetTaskWithPriority(string priorityName);
    }
}
