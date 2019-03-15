using System.Collections.Generic;
using System;
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
    public class PriorityService : IPriorityService
    {
        private IUnitOfWork db { get; set; }
        private IMapper mapper { get; set; }

        public PriorityService(IUnitOfWork uow)
        {
            db = uow;
            mapper = MapperConfig.GetMapperResult();
        }

        public IEnumerable<PriorityDTO> GetAllPriorities()
        {
            var priorities = db.Priorities.GetAll();

            return mapper.Map<IEnumerable<Priority>, IEnumerable<PriorityDTO>>(priorities);
        }

        public IEnumerable<TaskDTO> GetTaskWithPriority(int id)
        {
            //if (priorityName == null)
            //    throw new PriorityIsNullException("Current priority name is null");

            var tasks = db.Tasks.Find(t => (t.Priority.Id == id));
            var resultTasks = mapper.Map<IEnumerable<TaskInfo>, IEnumerable<TaskDTO>>(tasks);

            return resultTasks;
        }
    }
}
