using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BLL.DTO;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;

namespace BLL.Services
{
    public class StatusService
    {
        private IUnitOfWork db { get; set; }

        private IMapper mapper { get; set; }

        public StatusService(IUnitOfWork uow)
        {
            db = uow;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Person, PersonDTO>();
                cfg.CreateMap<Status, StatusDTO>();
                cfg.CreateMap<TaskInfo, TaskDTO>();
                cfg.CreateMap<Team, TeamDTO>();
                cfg.CreateMap<Priority, PriorityDTO>();
            });

            mapper = config.CreateMapper();
        }
        
        public IEnumerable<StatusDTO> GetAllStatuses()
        {
            var statuses = db.Statuses.GetAll();

            return mapper.Map<IEnumerable<Status>, IEnumerable<StatusDTO>>(statuses);
        }

        public IEnumerable<StatusDTO> GetActiveStatuses()
        {
            return GetAllStatuses().Where(s => s.Name != "Canceled" || s.Name != "Completed");
        }

        public IEnumerable<StatusDTO> GetNotActiveStatuses()
        {
            return GetAllStatuses().Where(s => s.Name == "Canceled" || s.Name == "Completed");
        }

        public void SetNewStatus(int taskId, string statusName)
        {
            if (string.IsNullOrWhiteSpace(statusName))
                            throw new ArgumentNullException("Name of status is null or empty", "statusName");
            
            Status status = db.Statuses.Find(s => (s.Name == statusName)).SingleOrDefault();

            TaskInfo task = db.Tasks.GetById(taskId);
            if (task == null)
                            throw new ArgumentException("Task wasn't found", "id");
            
            task.Status = status ?? throw new ArgumentException("Status with this name wasn't found", "statusName");
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
                case "Completed":
                    {
                        task.Progress = 100;
                        task.FinishDate = DateTime.Now;
                        break;
                    }
                default:
                    break;
            }

            db.Tasks.Update(task);
            db.Save();
        }
    }
}
