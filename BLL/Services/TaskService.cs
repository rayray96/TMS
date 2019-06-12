using AutoMapper;
using BLL.Configurations;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class TaskService : ITaskService
    {
        private IUnitOfWork db { get; set; }
        private IEmailService emailService { get; set; }
        private IMapper mapper { get; set; }

        private string EmailBody { get; set; }
        private string EmailSender { get; set; }
        private string EmailRecipient { get; set; }

        public TaskService(IUnitOfWork uow, IEmailService emailService)
        {
            db = uow;
            this.emailService = emailService;
            mapper = MapperConfig.GetMapperResult();
        }

        #region Main CRUD-operations for Tasks.

        public void DeleteTask(int taskId, string currentUserName)
        {
            TaskDTO task = mapper.Map<TaskInfo, TaskDTO>(db.Tasks.GetById(taskId));

            if (task == null)
                throw new TaskNotFoundException($"Task with this id \"{taskId}\" not found");

            PersonDTO managerDTO = mapper.Map<Person, PersonDTO>(db.People.Find(m => m.UserName == currentUserName).SingleOrDefault());

            if (task.AuthorId == managerDTO.Id)
            {
                db.Tasks.Delete(task.Id);
                db.Save();
            }
            else
            {
                throw new TaskAccessException($"Access error. You cannot delete this task \"{taskId}\"");
            }
        }

        public void CreateTask(EditTaskDTO task, string authorName, int assigneeId, string priority)
        {
            var newTask = EditTask(task, authorName, assigneeId, priority);

            db.Tasks.Create(newTask);
            db.Save();

            emailService.Send(EmailSender, EmailRecipient, EmailService.SUBJECT_NEW_TASK, EmailBody);
        }

        public void UpdateTask(EditTaskDTO task, int id, string authorName, int assigneeId, string priority)
        {
            TaskInfo taskForEdit = db.Tasks.GetById(id);

            if (taskForEdit != null)
            {
                TaskInfo newTask = EditTask(task, authorName, assigneeId, priority);

                newTask.Id = taskForEdit.Id;
                newTask.Progress = taskForEdit.Progress;
                newTask.StartDate = taskForEdit.StartDate;
                newTask.StatusId = taskForEdit.StatusId;
                newTask.FinishDate = taskForEdit.FinishDate;

                db.Tasks.Update(taskForEdit.Id, newTask);
                db.Save();

                emailService.Send(EmailSender, EmailRecipient, EmailService.SUBJECT_NEW_TASK, EmailBody);
            }
            else
                throw new TaskNotFoundException($"Current task \"{id}\"has not found");
        }

        public void UpdateStatus(int taskId, string statusName, int changerId)
        {
            Status status = db.Statuses.Find(s => (s.Name == statusName)).SingleOrDefault();
            if (status == null)
                throw new StatusNotFoundException($"Status with this name \"{statusName}\" has not found");

            TaskInfo task = db.Tasks.GetById(taskId);
            if (task == null)
                throw new TaskNotFoundException($"Task \"{taskId}\" wasn't found");

            Status taskStatus = db.Statuses.GetById(task.StatusId);

            if (task.AuthorId == changerId)
            {
                if ((taskStatus.Name == "Executed") && (statusName == "Completed"))
                {
                    task.Progress = 100;
                    task.FinishDate = DateTime.Now;
                }
                else if (statusName == "Canceled")
                {
                    task.Progress = 0;
                    task.StartDate = null;
                    task.FinishDate = null;
                }
                else if ((taskStatus.Name == "Canceled") && (statusName == "Not Started"))
                {
                }
                else
                    throw new StatuskAccessException($"This status cannot \"{statusName}\" belongs to Author \"{changerId}\"");
            }
            else if ((task.AssigneeId == changerId) && (taskStatus.Name != "Canceled"))
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
                        throw new StatuskAccessException($"This status cannot \"{statusName}\" belongs to Author \"{changerId}\"");
                }
                if (statusName != "Not started")
                    if (task.StartDate == null)
                        task.StartDate = DateTime.Now;

            }
            else
                throw new StatuskAccessException($"Current person \"{changerId}\" cannot change a status onto \"{statusName}\"");

            task.Status = status;

            db.Tasks.Update(task.Id, task);
            db.Save();

            // If assignee executed task manager should take email.
            if (status.Name == "Executed")
            {
                Person manager = db.People.GetById(task.AuthorId);
                Person worker = db.People.GetById(task.AssigneeId);

                EmailBody = string.Format(EmailService.BODY_EXECUTED_TASK, manager.FName + " " + manager.LName,
                   worker.FName + " " + worker.LName, task.Name);

                emailService.Send(worker.Email, manager.Email, EmailService.SUBJECT_EXECUTED_TASK, EmailBody);
            }
        }

        public TaskDTO GetTask(int id)
        {
            var condition = db.Tasks.Find(t => t.Id == id);
            var task = GetTasksWithCondition(condition).FirstOrDefault();

            return task;
        }

        public IEnumerable<TaskDTO> GetAllTasks()
        {
            var condition = db.Tasks.GetAll();
            var allTasks = GetTasksWithCondition(condition);

            return allTasks;
        }

        public IEnumerable<TaskDTO> GetTasksOfAuthor(string managerId)
        {
            var manager = db.People.Find(p => (p.UserId == managerId) && (p.Role == "Manager")).FirstOrDefault();
            if (manager == null)
                throw new ManagerNotFoundException($"Manager \"{managerId}\" is not found");

            var condition = db.Tasks.Find(t => t.AuthorId == manager.Id);
            var tasks = GetTasksWithCondition(condition);

            return tasks;
        }

        public IEnumerable<TaskDTO> GetTasksOfAssignee(string workerId)
        {
            var worker = db.People.Find(p => (p.UserId == workerId) && (p.Role == "Worker")).SingleOrDefault();
            if (worker == null)
                throw new WorkerNotFoundException($"Worker \"{workerId}\" is not found");

            var condition = db.Tasks.Find(t => t.AssigneeId == worker.Id);
            var tasks = GetTasksWithCondition(condition);

            return tasks;
        }

        #endregion

        #region Methods for getting progress.

        public int GetProgressOfTeam(string managerId)
        {
            var tasksOfTeam = GetTasksOfAuthor(managerId);

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

        #endregion

        #region Private methods.

        private TaskInfo EditTask(EditTaskDTO task, string authorName, int assigneeId, string priority)
        {
            if (task.Deadline < DateTime.Now)
                throw new DateIsWrongException($"Deadline date \"{task.Deadline}\" cannot be less than current date \"{DateTime.Now}\"");

            PersonDTO authorDTO = mapper.Map<Person, PersonDTO>(db.People.Find(p => p.UserName == authorName).SingleOrDefault());
            if (authorDTO == null)
                throw new PersonNotFoundException($"Author \"{authorName}\" has not found");

            PersonDTO assigneeDTO = mapper.Map<Person, PersonDTO>(db.People.Find(p => p.Id == assigneeId).SingleOrDefault());
            if (assigneeDTO == null)
                throw new PersonNotFoundException($"Assignee \"{assigneeId}\" has not found");

            PriorityDTO priorityDTO = mapper.Map<Priority, PriorityDTO>(db.Priorities.Find(p => p.Name == priority).SingleOrDefault());
            if (priorityDTO == null)
                throw new PriorityNotFoundException($"Priority \"{priority}\" has not known");

            StatusDTO status = mapper.Map<Status, StatusDTO>(db.Statuses.Find(s => (s.Name == "Not Started")).SingleOrDefault());
            if (status == null)
                throw new StatusNotFoundException("Status \"Not Started\" has not found in database");

            var newTask = new TaskInfo
            {
                Name = task.Name,
                Description = task.Description,
                PriorityId = priorityDTO.Id,
                AuthorId = authorDTO.Id,
                AssigneeId = assigneeDTO.Id,
                StatusId = status.Id,
                Progress = 0,
                StartDate = null,
                FinishDate = null,
                Deadline = task.Deadline,
            };
            // For sending email to assignee when task created or updated.
            EmailBody = string.Format(EmailService.BODY_NEW_TASK,
                               assigneeDTO.FName + " " + assigneeDTO.LName, task.Name, authorDTO.FName + " " + authorDTO.LName);
            EmailSender = authorDTO.Email;
            EmailRecipient = assigneeDTO.Email;

            return newTask;
        }

        private IEnumerable<TaskDTO> GetTasksWithCondition(IEnumerable<TaskInfo> condition)
        {
            IEnumerable<TaskDTO> tasks = condition
                            .Join(db.People.GetAll(), x => x.AssigneeId, y => y.Id, (x, y) => new { x, Assignee = y.FName + " " + y.LName })
                            .Join(db.People.GetAll(), x => x.x.AuthorId, y => y.Id, (x, y) => new { x.x, x.Assignee, Author = y.FName + " " + y.LName })
                            .Join(db.Statuses.GetAll(), x => x.x.StatusId, y => y.Id, (x, y) => new { x.x, x.Assignee, x.Author, Status = y.Name })
                            .Join(db.Priorities.GetAll(), x => x.x.PriorityId, y => y.Id, (x, y) => new { x.x, x.Assignee, x.Author, x.Status, Priority = y.Name })
                            .Select(n => new TaskDTO
                            {
                                Assignee = n.Assignee,
                                Author = n.Author,
                                Status = n.Status,
                                Priority = n.Priority,
                                Id = n.x.Id,
                                Deadline = n.x.Deadline,
                                Description = n.x.Description,
                                FinishDate = n.x.FinishDate,
                                Name = n.x.Name,
                                Progress = n.x.Progress,
                                StartDate = n.x.StartDate,
                                AssigneeId = n.x.AssigneeId
                            });

            return tasks;
        }

        #endregion

    }
}
