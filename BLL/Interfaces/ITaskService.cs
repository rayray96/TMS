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
        /// Delete task from database.
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
        /// <param name="priority">Priority for current task</param>
        /// <param name="deadline">The end date of current task</param>
        /// ///<exception cref="ArgumentNullException">Name of author is null or empty</exception>
        /// ///<exception cref="Exception">Problem with status new in database. </exception>  
        void CreateTask(TaskDTO task, string authorName, string assigneeName, string priority, DateTime? deadline);

        /// <summary>
        /// Update task by author.
        /// </summary>
        /// <param name="task">Task with edits</param>
        /// <param name="authorName">Name of author of the current task</param>
        /// ///<exception cref="ArgumentNullException">Name of author is null or empty</exception>  
        void UpdateTask(TaskDTO task, string authorName);

        /// <summary>
        /// Set new status to task.
        /// </summary>
        /// <param name="taskid">Id of task that need to set new status</param>
        /// <param name="statusName">Name of new status for task</param>
        /// <param name="changerId">Id of the asignee or the author for current task</param>
        /// ///<exception cref="ArgumentNullException">Name of status is null or empty</exception>
        /// ///<exception cref="ArgumentException">task for edit wasn't found</exception>   
        void UpdateStatus(int taskId, string statusName, int changerId);

        /// <summary>
        /// Get task with Id.
        /// </summary>
        /// <param name="id">Id of task</param>
        /// <returns>Task</returns>
        TaskDTO GetTask(int id);

        /// <summary>
        /// Get tasks of team by Manager Id.
        /// </summary>
        /// <param name="managerId">Id of manager of team</param>
        /// ///<exception cref="ArgumentException">Manager with this Id wasn't found</exception>   
        /// <returns>Enumeration of tasks</returns>
        IEnumerable<TaskDTO> GetTasksOfTeam(string managerId);

        /// <summary>
        /// Get tasks of team which are inactive.
        /// </summary>
        /// <param name="teamId">Id of team</param>
        /// <returns>Enumeration of tasks</returns>
        IEnumerable<TaskDTO> GetInactiveTasks(int teamId);

        /// <summary>
        /// Get tasks of team where status is completed.
        /// </summary>
        /// <param name="teamId">Id of team</param>
        /// <returns>Enumerations of tasks</returns>
        IEnumerable<TaskDTO> GetCompletedTasks(int teamId);

        /// <summary>
        /// Get all tasks of shown assignee.
        /// </summary>
        /// <param name="id">String Id of assignee</param>
        /// <returns>Enumerations of tasks</returns>
        IEnumerable<TaskDTO> GetTasksOfAssignee(string id);

        /// <summary>
        /// Get all tasks of shown author.
        /// </summary>
        /// <param name="id">String Id of assignee</param>
        /// <returns>Enumerations of tasks</returns>
        IEnumerable<TaskDTO> GetTasksOfAuthor(string id);

        /// <summary>
        /// Get all tasks.
        /// </summary>
        /// <returns>Enumerations of tasks</returns>
        IEnumerable<TaskDTO> GetAllTasks();

        /// <summary>
        /// Get common progress of tasks of current team.
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns>Progress of team</returns>
        int GetProgressOfTeam(string managerId);

        /// <summary>
        /// Get common progress of all tasks.
        /// </summary>
        /// <returns>Progress of all team</returns>
        int GetProgressOfAllTasks();
    }
}

