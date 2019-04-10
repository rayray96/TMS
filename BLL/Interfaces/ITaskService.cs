using BLL.DTO;
using BLL.Exceptions;
using System;
using System.Collections.Generic;

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
        /// ///<exception cref="TaskNotFoundException">Task with this Id has not found</exception>
        /// ///<exception cref="TaskAccessException">If person who want to delete task is not its author</exception>
        void DeleteTask(int taskId, string currentUserName);

        /// <summary>
        /// Add task to database.
        /// </summary>
        /// <param name="task">Task to adding</param>
        /// <param name="authorName">Name of task author</param>
        /// <param name="priority">Priority for current task</param>
        /// <param name="deadline">The end date of current task</param>
        /// ///<exception cref="DateIsWrongException">Deadline date cannot be less than current date</exception>
        /// ///<exception cref="PersonNotFoundException">Author or assigneehas not found</exception>  
        /// ///<exception cref="PriorityNotFoundException">Priority has not known</exception>  
        /// ///<exception cref="StatusNotFoundException">Status "Not Started" has not found in database</exception>  
        void CreateTask(EditTaskDTO task, string authorName, int assigneeId, string priority);

        /// <summary>
        /// Update task by author.
        /// </summary>
        /// <param name="task">Task to adding</param>
        /// <param name="authorName">Name of task author</param>
        /// <param name="id">Id of task author</param>
        /// <param name="priority">Priority for current task</param>
        /// <param name="deadline">The end date of current task</param>
        /// ///<exception cref="TaskNotFoundException">Current task has not found</exception>
        /// ///<exception cref="DateIsWrongException">Deadline date cannot be less than current date</exception>
        /// ///<exception cref="PersonNotFoundException">Author or assigneehas not found</exception>  
        /// ///<exception cref="PriorityNotFoundException">Priority has not known</exception>  
        /// ///<exception cref="StatusNotFoundException">Status "Not Started" has not found in database</exception>  
        void UpdateTask(EditTaskDTO task, int id, string authorName, int assigneeId, string priority);

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
        /// <param name="managerId">String Id of author</param>
        /// ///<exception cref="ManagerNotFoundException">Manager with this Id has not found</exception>   
        /// <returns>Enumeration of tasks</returns>
        IEnumerable<TaskDTO> GetTasksOfAuthor(string managerId);

        /// <summary>
        /// Get tasks of team by Worker Id.
        /// </summary>
        /// <param name="workerId">String Id of assignee</param>
        /// ///<exception cref="WorkerNotFoundException">Worker with this Id has not found</exception>   
        /// <returns>Enumerations of tasks</returns>
        IEnumerable<TaskDTO> GetTasksOfAssignee(string workerId);

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

