using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BLL.Configurations;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;

namespace BLL.Services
{
    public class TaskService : ITaskService
    {
        private IUnitOfWork db { get; set; }
        private IMapper mapper { get; set; }

        public TaskService(IUnitOfWork uow)
        {
            db = uow;
            mapper = MapperConfig.GetMapperResult();
        }

        #region Main CRUD-operations for Tasks.

        public void DeleteTask(int taskId, string currentUserName)
        {
            TaskDTO task = mapper.Map<TaskInfo, TaskDTO>(db.Tasks.GetById(taskId));

            if (task == null)
                throw new TaskNotFoundException("Task with this id not found");

            if (task.Author.Name == currentUserName)
            {
                db.Tasks.Delete(task.Id);
                db.Save();
            }
            else
            {
                throw new TaskAccessException("Access error. You cannot delete this task");
            }
        }

        public void CreateTask(TaskDTO task, string authorName, string assigneeName, string priority, string deadline)
        {
            if (string.IsNullOrWhiteSpace(authorName))
                throw new PersonNotFoundException("Author is not shown");

            PersonDTO authorDTO = mapper.Map<Person, PersonDTO>(db.People.Find(p => p.Name == authorName).Single());

            PersonDTO assigneeDTO;
            if (string.IsNullOrEmpty(assigneeName))
                assigneeDTO = authorDTO;
            else
                assigneeDTO = mapper.Map<Person, PersonDTO>(db.People.Find(p => p.Name == assigneeName).Single());

            StatusDTO status = mapper.Map<Status, StatusDTO>(db.Statuses.Find(s => (s.Name == "Not Started")).SingleOrDefault());
            if (status == null)
                throw new StatusNotFoundException("Status \"New\" was not found in database");


            PriorityDTO prior;
            if (priority != null)
                prior = mapper.Map<Priority, PriorityDTO>(db.Priorities.Find(s => (s.Name == priority)).SingleOrDefault());
            else
                prior = null;


            DateTime? endDate;
            if (deadline != null)
                endDate = Convert.ToDateTime(deadline);
            else
                endDate = null;

            var newTask = new TaskDTO
            {
                Name = task.Name,
                Description = task.Description,
                PriorityId = prior,
                Author = authorDTO,
                Assignee = assigneeDTO,
                Status = status,
                Progress = 0,
                StartDate = null,
                FinishDate = null,
                Deadline = endDate,
            };

            db.Tasks.Create(mapper.Map<TaskDTO, TaskInfo>(newTask));
            db.Save();
        }

        public void UpdateTask(TaskDTO task, string authorName)
        {
            TaskInfo taskForEdit = db.Tasks.GetById(task.Id);

            if (taskForEdit != null)
            {
                if ((authorName == null))
                    throw new ManagerNotFoundException("The author name cannot be null");

                if (taskForEdit.Author.Name == authorName)
                {
                    Person author = db.People.Find(p => (p.Name == authorName)).Single();

                    taskForEdit.Priority.Id = task.PriorityId.Id;
                    taskForEdit.Status.Name = task.Status.Name;
                    taskForEdit.StartDate = task.StartDate;
                    taskForEdit.FinishDate = task.FinishDate;
                    taskForEdit.Description = task.Description;
                    taskForEdit.Deadline = task.Deadline;
                }

                db.Tasks.Update(taskForEdit);
                db.Save();
            }
        }

        public TaskDTO GetTask(int id)
        {
            var result = db.Tasks.GetById(id);
            return mapper.Map<TaskInfo, TaskDTO>(result);
        }

        public IEnumerable<TaskDTO> GetAllTasks()
        {
            var tasks = db.Tasks.GetAll();
            IEnumerable<TaskDTO> resulttasks = mapper.Map<IEnumerable<TaskInfo>, IEnumerable<TaskDTO>>(tasks);

            return resulttasks;
        }

        public IEnumerable<TaskDTO> GetTasksOfTeam(string managerId)
        {
            var manager = db.People.Find(p => p.UserId == managerId).SingleOrDefault();
            if (manager == null)
                throw new ManagerNotFoundException("Manager is not found");

            IEnumerable<TaskInfo> tasks = db.Tasks.Find(t => ((t.Author.Id == manager.Id) &&
                                            (t.Assignee.Id != manager.Id))).OrderBy(tsk => tsk.Assignee.Name).ToList();

            IEnumerable<TaskDTO> resulttasks = mapper.Map<IEnumerable<TaskInfo>, IEnumerable<TaskDTO>>(tasks);
            return resulttasks;
        }

        public IEnumerable<TaskDTO> GetInactiveTasks(int teamId)
        {
            var manager = db.Teams.GetById(teamId);
            var tasks = db.Tasks.Find(t => ((t.Author.Id == manager.Id) && ((t.Deadline < DateTime.Now) || (t.Deadline == null))));

            return mapper.Map<IEnumerable<TaskInfo>, IEnumerable<TaskDTO>>(tasks);
        }

        public IEnumerable<TaskDTO> GetCompletedTasks(int teamId)
        {
            var manager = db.Teams.GetById(teamId);
            var tasks = db.Tasks.Find(t => ((t.Author.Id == manager.Id))
                                        && (t.Status.Name == "Completed"));

            return mapper.Map<IEnumerable<TaskInfo>, IEnumerable<TaskDTO>>(tasks);
        }

        public IEnumerable<TaskDTO> GetTasksOfAssignee(string id)
        {
            IEnumerable<TaskInfo> tasks = db.Tasks.Find(t => (t.Assignee.UserId == id))
                                               .OrderByDescending(t => t.Progress);
            IEnumerable<TaskDTO> result = mapper.Map<IEnumerable<TaskInfo>, IEnumerable<TaskDTO>>(tasks);

            return result;
        }

        public IEnumerable<TaskDTO> GetTasksOfAuthor(string id)
        {
            IEnumerable<TaskInfo> tasks = db.Tasks.Find(t => (t.Author.UserId == id))
                                               .OrderByDescending(t => t.Progress);
            IEnumerable<TaskDTO> result = mapper.Map<IEnumerable<TaskInfo>, IEnumerable<TaskDTO>>(tasks);

            return result;
        }

        #endregion

        public void UpdateStatus(int taskId, string statusName, int changerId)
        {
            if (string.IsNullOrWhiteSpace(statusName))
                throw new StatusNotFoundException("Name of status is null or empty");

            Status status = db.Statuses.Find(s => (s.Name == statusName)).SingleOrDefault();

            TaskInfo task = db.Tasks.GetById(taskId);
            if (task == null)
                throw new TaskNotFoundException("Task wasn't found");

            if (task.AuthorId == changerId)
            {
                if ((task.Status.Name == "Executed") && (statusName == "Completed"))
                {
                    task.Progress = 100;
                    task.FinishDate = DateTime.Now;
                }
                else if (statusName == "Canceled")
                    task.Progress = 0;
                else
                    throw new StatuskAccessException("This is status cannot belong to Author");
            }
            else if (task.AssigneeId == changerId)
            {
                switch (statusName)
                {
                    case "Not started":
                        {
                            task.StartDate = null;
                            task.Progress = 0;
                            break;
                        }
                    case "In Progress":
                        {
                            task.StartDate = DateTime.Now;
                            task.Progress = 20;
                            break;
                        }
                    case "Test":
                        {
                            task.Progress = 40;
                            break;
                        }
                    case "Almost Ready":
                        {
                            task.Progress = 60;
                            break;
                        }
                    case "Executed":
                        {
                            task.Progress = 80;
                            break;
                        }
                    default:
                        throw new StatuskAccessException("This status cannot belongs to Author");
                }
            }
            else
                throw new StatuskAccessException("Current person cannot change a status");
            task.Status = status ?? throw new StatusNotFoundException("Status with this name was not found");

            db.Tasks.Update(task);
            db.Save();
        }

        public int GetProgressOfTeam(string managerId)
        {
            var tasksOfTeam = GetTasksOfTeam(managerId);

            int sumProgress = 0;
            int counter = 0;

            foreach (var task in tasksOfTeam)
            {
                if (task.Progress.HasValue)
                    sumProgress += task.Progress.Value;

                counter++;
            }

            sumProgress = (counter != 0) ? (sumProgress / counter) : 0;

            return sumProgress;
        }

        public int GetProgressOfAllTasks()
        {
            var tasks = GetAllTasks();

            int sumProgress = 0;
            int counter = 0;

            foreach (var task in tasks)
            {
                if (task.Progress.HasValue)
                    sumProgress += task.Progress.Value;

                counter++;
            }

            sumProgress = (counter != 0) ? (sumProgress / counter) : 0;

            return sumProgress;
        }
    }
}
