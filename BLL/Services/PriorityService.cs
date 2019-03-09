using System.Collections.Generic;
using System;
using System.Linq;
using AutoMapper;
using BLL.DTO;
using BLL.Configurations;
using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Entities;

namespace BLL.Services
{
    class PriorityService : IPriorityService
    {
        private IUnitOfWork db { get; set; }
        private IMapper mapper { get; set; }

        public PriorityService(IUnitOfWork uow)
        {
            db = uow;
            mapper = MapperConfig.MapperResult();
        }

        public IEnumerable<PriorityDTO> GetAllPriorities()
        {
            var priorities = db.Priorities.GetAll();

            return mapper.Map<IEnumerable<Priority>, IEnumerable<PriorityDTO>>(priorities);
        }

        public IEnumerable<TaskDTO> GetTaskWithPriority(string priorityName)
        {
            if (priorityName == null)
                throw new ArgumentNullException("Current priority name is null", "priorityName");

            var tasks = db.Tasks.Find(t => (t.Priority.Name == priorityName));
            var resultTasks = mapper.Map<IEnumerable<TaskInfo>, IEnumerable<TaskDTO>>(tasks);

            return resultTasks;
        }
    }
}
